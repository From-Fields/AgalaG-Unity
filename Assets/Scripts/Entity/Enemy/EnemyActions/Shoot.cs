using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[Serializable]
public class Shoot: iEnemyAction
{
    //Attributes
    [SerializeField]
    private float _timeout;
    private bool _isDone;

    //Constructors
    public Shoot(float timeout)
    {
        this._timeout = timeout;
        this._isDone = false;
    }

    //Methods
    private IEnumerator WaitForTimeout()
    {
        yield return new WaitForSeconds(_timeout);
        this._isDone = true;
        yield return null;
    }

    #region Interface Implementation
    public bool CheckCondition(iEnemy target) => _isDone;
    public void FixedUpdate(iEnemy target) { return; }
    public void Update(iEnemy target) => target.Shoot();
    public void OnStart(iEnemy target)
    {
        this._isDone = false;
        target.StartCoroutine(WaitForTimeout());
    }
    public void OnFinish(iEnemy target) { return; }
    #endregion
}