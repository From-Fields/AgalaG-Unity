using System;
using System.Collections.Generic;

public interface iEnemyAction
{
	public bool CheckCondition(iEnemy target);
    public void Update(iEnemy target);
    public void FixedUpdate(iEnemy target);
    public void OnStart(iEnemy target);
    public void OnFinish(iEnemy target);
}

[Serializable]
public struct EnemyAction
{
    [UnityEngine.SerializeField, UnityEngine.SerializeReference]
    public iEnemyAction action;
}

