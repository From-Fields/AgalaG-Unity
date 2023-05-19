using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRepository : SingletonMonoBehaviour<PrefabRepository>
{
    [Header("Enemies")]
    [SerializeField]
    private EnemyKamikaze _kamikazePrefab;
    [SerializeField]
    private EnemyBumblebee _bumblebeePrefab;
    [SerializeField]
    private EnemyGemini _geminiPrefab;
    [SerializeField]
    private EnemyGeminiChild _geminiChildPrefab;
    [Header("Weapons")]
    [SerializeField]
    private DefaultWeapon _defaultWeaponPrefab;

    public GameObject GetPrefabOfType(Type t) {
        if(t == typeof(EnemyKamikaze)) {
            return _kamikazePrefab.gameObject;
        }
        if(t == typeof(EnemyBumblebee)) {
            return _bumblebeePrefab.gameObject;
        }
        if(t == typeof(EnemyGemini)) {
            return _geminiPrefab.gameObject;
        }
        if(t == typeof(EnemyGeminiChild)) {
            return _geminiChildPrefab.gameObject;
        }

        if(t == typeof(iEnemy)) {
            Debug.Log(typeof(iEnemy));
        }

        if(t == typeof(DefaultWeapon)) {
            return _defaultWeaponPrefab.gameObject;
        }

        return null;
    }
}