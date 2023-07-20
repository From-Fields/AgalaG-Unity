using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Enemy<T>: MonoBehaviour, iEnemy, iPoolableEntity<T> where T: Enemy<T>
{
    //Attributes
    public int score;

    //Damage
    [Header("Damage")][SerializeField]
    protected int _defaultCollisionDamage = 1;
    protected int _collisionDamage;

    //Speed
    [Header("Movement")][SerializeField]
    protected float _defaultSpeed = 100;
    protected float _currentSpeed;

    //Acceleration
    [SerializeField]
    public float _defaultAcceleration = 10;
    public float _currentAcceleration;

    //Drops
    [SerializeField]
    private PowerUp _droppedItem = null;

    protected bool _isDead;

    //Actions
    [SerializeField]
    protected Queue<iEnemyAction> _actionQueue;
    protected iEnemyAction _startingAction;
    protected iEnemyAction _timeoutAction;
    protected iEnemyAction _currentAction;

    //References
    public abstract Rigidbody2D Rigidbody { get; }
    protected AudioManager _audioManager;

    //Properties
    public bool IsDead => this._isDead;

    //Events
    public Action<int> onDeath;
    public Action onRelease;

    //Methods
    public void ExecuteNextAction()
    {
        if(this._actionQueue.Count > 0)
            this.SwitchAction(this._actionQueue.Dequeue());
        else if(_currentAction != _timeoutAction)
            this.SwitchAction(_timeoutAction);
        else
            this.SwitchAction(null);
    }
    public void ExecuteStartingAction() => this.SwitchAction(this._startingAction);
    public void ExecuteTimeoutAction() => this.SwitchAction(this._timeoutAction);
    protected void SwitchAction(iEnemyAction action)
    {
        if(_isDead)
            return;

        this._currentAction?.OnFinish(this);
        this._currentAction = action;
        this._currentAction?.OnStart(this);

        if(_currentAction == null)
            Reserve();
    }
    public void Initialize(Queue<iEnemyAction> actionQueue, iEnemyAction startingAction, iEnemyAction timeoutAction, Vector2 startingPoint, PowerUp drop = null)
    {
        if(actionQueue == null || timeoutAction == null)
            throw new ArgumentNullException("Action queue and Timeout action may not be null");

        this._actionQueue = actionQueue;
        this._startingAction = startingAction;
        this._timeoutAction = timeoutAction;
        this.Rigidbody.position = startingPoint;
        this.transform.position = startingPoint;

        this._droppedItem = drop;

        _audioManager = GetComponentInChildren<AudioManager>(true);
        _audioManager.PlaySound(EntityAudioType.Movement, looping: true);
        this.SubInitialize();

        this.gameObject.SetActive(true);

        if(this._startingAction != null)
            this.SwitchAction(this._startingAction);
        else
            this.ExecuteNextAction();
    }
    public void Reserve()
    {
        this._actionQueue = null;
        this._startingAction = null;
        this._timeoutAction = null;

        this.onDeath = null;
        
        this._isDead = true;
        this.Rigidbody.position = Vector3.zero;
        this.transform.position = Vector3.zero;
        this.Rigidbody.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
        this.SubReserve();
        this.ReserveToPool();

        this.onRelease?.Invoke();
    }

    //Abstract Methods
    protected abstract void SubInitialize();
    protected abstract void ReserveToPool();

    //Virtual Methods
    protected virtual void SubReserve() { }
    protected virtual void SubUpdate() { }
    protected virtual void SubFixedUpdate() { }
    protected virtual void OnCollision(Collision2D other) {
        Entity entity = other.gameObject.GetComponentInChildren<Entity>();

        // Debug.Log("collision");

        if(entity != null) {
            entity.TakeDamage(this._collisionDamage);
            this.Die();
        }
    }

    //Unity Hooks
    public void Update()
    {
        if(_isDead || _currentAction == null)
            return;

        if(_currentAction.CheckCondition(this))
            ExecuteNextAction();

        _currentAction?.Update(this);
        SubUpdate();
    }
    public void FixedUpdate()
    {
        if(_isDead || _currentAction == null)
            return;
        
        if(!_currentAction.CheckCondition(this))
            _currentAction.FixedUpdate(this);
        SubFixedUpdate();
    }
    public void OnCollisionEnter2D(Collision2D other)  {
        if(_isDead)
            return;

        OnCollision(other);
    }

    #region Interface Implementation
    //iEntity
    public abstract int health { get; }
    public abstract Vector2 CurrentVelocity { get; }
    public abstract Vector2 Position { get; }

    public abstract void Move(Vector2 direction, float speed, float acceleration);
    public abstract void Stop();
    public abstract void Shoot();
    public abstract void TakeDamage(int damage);
    public void Die()
    {
        onDeath?.Invoke(this.score);
        _audioManager.PlaySound(EntityAudioType.Death);
        _audioManager.StopSound(EntityAudioType.Movement);
        
        if(_droppedItem != null) {
            Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-0.9f, 0.9f), UnityEngine.Random.Range(-0.9f, 0.9f)).normalized;
            SingletonObjectPool<PickUp>.Instance.Pool.Get().Initialize(_droppedItem, Rigidbody.transform.position, randomDirection);
        }

        Reserve();
    }

    //iEnemy
    public float DesiredSpeed => _currentSpeed;
    public float CurrentAcceleration => _currentAcceleration;

    //iPoolableEntity
	public abstract T OnCreate();
	public virtual Action<T> onGetFromPool { get; }
	public virtual Action<T> onReleaseToPool { get; }
    public abstract IObjectPool<T> Pool { get; }
    #endregion
}