using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveUnit<T> where T: Enemy<T> 
{
    private T _enemy;
    private Vector2 _startingPoint;
    private iEnemyAction _startingAction;
    private iEnemyAction _timeoutAction;
    private Queue<iEnemyAction> _actions;
    private float _timeout;

    public bool IsDone => _enemy.IsDead;

    public WaveUnit(Vector2 startingPoint, iEnemyAction startingAction, iEnemyAction timeoutAction, Queue<iEnemyAction> actions, Action<int> onDeath = null, float timeout = -1)
    {
        _enemy = EntityPool<T>.Instance.Pool.Get();
        _startingPoint = startingPoint;
        _startingAction = startingAction;
        _timeoutAction = timeoutAction;
        _actions = actions;
        _enemy.onDeath += onDeath;
        _timeout = timeout;
    }

    public void Initialize() 
    {
        _enemy.Initialize(_actions, _startingAction, _timeoutAction, _startingPoint);

        if(_timeout > 0)
            _enemy.StartCoroutine(SetTimeOut());
    }
    private IEnumerator SetTimeOut()
    {
        yield return new WaitForSeconds(_timeout);

        _enemy.ExecuteTimeoutAction();
    }
}