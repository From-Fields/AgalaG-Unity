using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveUnit<T>: iWaveUnit where T: Enemy<T> 
{
    private T _enemy;
    private Vector2 _startingPoint;
    private iEnemyAction _startingAction;
    private iEnemyAction _timeoutAction;
    private Queue<iEnemyAction> _actions;

    private float _timeout;
    private bool _hasTimedOut;

    public Action<iWaveUnit> onUnitReleased { get; set; }

    public WaveUnit(
        Vector2 startingPoint, iEnemyAction startingAction, iEnemyAction timeoutAction, Queue<iEnemyAction> actions, 
        Action<int> onDeath = null, Action onRelease = null, float timeout = -1
    ) {
        _enemy = EntityPool<T>.Instance.Pool.Get();
        _startingPoint = startingPoint;
        
        _startingAction = startingAction;
        _timeoutAction = timeoutAction;
        _actions = actions;

        _enemy.onDeath += onDeath;
        _enemy.onRelease += onRelease;
        _enemy.onRelease += () => onUnitReleased?.Invoke(this);

        _timeout = timeout;
        _hasTimedOut = false;
    }

    public void Initialize()
    {
        _enemy.Initialize(_actions, _startingAction, _timeoutAction, _startingPoint);

        if(_timeout > 0)
            CoroutineRunner.Instance.CallbackTimer(_timeout, ExecuteTimeoutAction);
    }
    public void ExecuteTimeoutAction()
    {
        if(_hasTimedOut)
            return;

        _hasTimedOut = true;

        _enemy.ExecuteTimeoutAction();
    }
}