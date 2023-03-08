using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[Serializable]
public class WaitSeconds: iEnemyAction
{
    //Attributes
    [SerializeField]
    private float _timeout;
    private bool _isDone;

    //Constructors
    public WaitSeconds(float timeout)
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
    public void Update(iEnemy target) { return; }
    public void OnStart(iEnemy target)
    {
        Debug.Log("Waiting");
        this._isDone = false;
        target.StartCoroutine(WaitForTimeout());
    }
    public void OnFinish(iEnemy target) { return; }
    #endregion
}