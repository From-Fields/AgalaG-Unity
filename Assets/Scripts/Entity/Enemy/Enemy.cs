using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Enemy<T>: MonoBehaviour, iEnemy, iPoolableEntity<T> where T: Enemy<T>
{
    //Attributes
    public int score;

    //Speed
    [SerializeField]
    protected float _defaultSpeed = 100;
    protected float _currentSpeed;    

    //Acceleration
    [SerializeField]
    public float _defaultAcceleration = 10;
    public float _currentAcceleration;

    protected bool _isDead;

    //Actions
    [SerializeField]
    protected Queue<iEnemyAction> _actionQueue;
    protected iEnemyAction _startingAction;
    protected iEnemyAction _timeoutAction;
    protected iEnemyAction _currentAction;

    //References
    public abstract Rigidbody2D Rigidbody { get; }

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
    public void Initialize(Queue<iEnemyAction> actionQueue, iEnemyAction startingAction, iEnemyAction timeoutAction, Vector2 startingPoint)
    {
        if(actionQueue == null || timeoutAction == null)
            throw new ArgumentNullException("Action queue and Timeout action may not be null");

        this.SubInitialize();

        this._actionQueue = actionQueue;
        this._startingAction = startingAction;
        this._timeoutAction = timeoutAction;
        this.transform.position = startingPoint;

        this.gameObject.SetActive(true);

        if(this._startingAction != null)
            this.SwitchAction(this._startingAction);
        else
            this.ExecuteNextAction();
    }
    protected void OnReserve()
    {
        this.onRelease?.Invoke();
        
        this._actionQueue = null;
        this._startingAction = null;
        this._timeoutAction = null;

        this.onDeath = null;
        this.onRelease = null;
        
        this._isDead = true;
        this.transform.position = Vector3.zero;
        this.Rigidbody.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    //Abstract Methods
    protected abstract void SubInitialize();
    public abstract void Reserve();

    //Unity Hooks
    public void Update()
    {
        if(_isDead || _currentAction == null)
            return;

        if(_currentAction.CheckCondition(this))
            ExecuteNextAction();

        _currentAction?.Update(this);

    }
    public void FixedUpdate()
    {
        if(_isDead || _currentAction == null)
            return;
        
        if(!_currentAction.CheckCondition(this))
            _currentAction.FixedUpdate(this);
    }

    #region Interface Implementation
    //iEntity
    public abstract int health { get; }
    public abstract Vector2 CurrentVelocity { get; }
    public abstract Vector2 Position { get; }

    public abstract void Move(Vector2 direction, float speed, float acceleration);
    public abstract void Shoot();
    public abstract void TakeDamage(int damage);
    public void Die()
    {
        onDeath?.Invoke(this.score);
        Reserve();
    }

    //iEnemy
    public float DesiredSpeed => _currentSpeed;
    public float CurrentAcceleration => _currentAcceleration;

    //iPoolableEntity
	public abstract T OnCreate();
	public abstract Action<T> onGetFromPool { get; }
	public virtual Action<T> onReleaseToPool => (obj) => obj.OnReserve();
    public abstract IObjectPool<T> Pool { get; }
    #endregion
}