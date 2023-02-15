using System;
using System.Collections.Generic;

public interface iEnemyAction
{
	public bool CheckCondition(Enemy target);
    public void Update(Enemy target);
    public void FixedUpdate(Enemy target);
    public void OnStart(Enemy target);
    public void OnFinish(Enemy target);
}

[Serializable]
public struct EnemyAction
{
    [UnityEngine.SerializeField, UnityEngine.SerializeReference]
    public iEnemyAction action;
}

