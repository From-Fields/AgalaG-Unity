using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy: MonoBehaviour, Entity
{
    //Attributes
    public int score;

    protected float _currentSpeed;    
    [SerializeField]
    protected float _defaultSpeed;

    [SerializeField]
    protected Queue<iEnemyAction> _actionQueue;
    protected iEnemyAction _startingAction;
    protected iEnemyAction _timeoutAction;
    protected iEnemyAction _currentAction;

    
    public float acceleration;
    public float DesiredSpeed => _currentSpeed;
    public abstract Rigidbody2D Rigidbody { get; }

    //Methods
    public void ExecuteNextAction()
    {
        if(this._actionQueue.Count > 0)
            this.SwitchAction(this._actionQueue.Dequeue());
        else
            this.SwitchAction(null);
    }
    public void ExecuteStartingAction() => this.SwitchAction(this._startingAction);
    public void ExecuteTimeoutAction() => this.SwitchAction(this._timeoutAction);
    protected void SwitchAction(iEnemyAction action)
    {
        this._currentAction?.OnFinish(this);
        this._currentAction = action;
        this._currentAction?.OnStart(this);
    }
    public void Initialize(Queue<iEnemyAction> actionQueue, iEnemyAction startingAction, iEnemyAction timeoutAction, Vector2 startingPoint)
    {
        if(actionQueue == null || timeoutAction == null)
            throw new ArgumentNullException("Action queue and Timeout action may not be null");

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
    public void Reserve()
    {
        this._actionQueue = null;
        this._startingAction = null;
        this._timeoutAction = null;
        this.transform.position = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    //Unity Hooks
    public void Update()
    {
        if(_currentAction == null)
            return;

        if(_currentAction.CheckCondition(this))
            ExecuteNextAction();

        _currentAction?.Update(this);

    }
    public void FixedUpdate()
    {
        if(_currentAction == null)
            return;
        
        if(!_currentAction.CheckCondition(this))
            _currentAction.FixedUpdate(this);
    }

    #region Interface Implementation
    public abstract int health { get; }
    public abstract Vector2 CurrentVelocity { get; }
    public abstract Vector2 Position { get; }
    public abstract void Move(Vector2 direction, float speed, float acceleration);
    public abstract void Shoot();
    public abstract void TakeDamage(int damage);
    public abstract void Die();
    #endregion
}