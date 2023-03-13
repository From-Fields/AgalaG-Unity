using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRepository : SingletonMonoBehaviour<PrefabRepository>
{
    [SerializeField]
    private EnemyKamikaze _kamikazePrefab;

    public GameObject GetPrefabOfType(Type t) {
        if(t == typeof(EnemyKamikaze)) {
            return _kamikazePrefab.gameObject;
        }

        if(t == typeof(iEnemy)) {
            Debug.Log(typeof(iEnemy));
        }

        return null;
    }
}