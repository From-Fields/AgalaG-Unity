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
    [SerializeField]
    private Weapon _missileWeaponPrefab, _multishotWeaponPrefab;
    [Header("PowerUps")]
    [SerializeField]
    private PickUp _pickupPrefab;
    [SerializeField]
    private Sprite _shieldPowerUp, _repairPowerUp, _multishotPowerUp, _missilePowerUp;

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
        if(t == typeof(Missile)) {
            return _missileWeaponPrefab.gameObject;
        }
        if(t == typeof(TripleMachineGun)) {
            return _multishotWeaponPrefab.gameObject;
        }

        if(t == typeof(PickUp)) {
            return _pickupPrefab.gameObject;
        }

        return null;
    }

    public Sprite GetPowerUpOfType(Type t) {
        if(t == typeof(ShieldPowerUp)) {
            return _shieldPowerUp;
        }
        if(t == typeof(RepairPowerUp)) {
            return _repairPowerUp;
        }
        if(t == typeof(MissilePowerUp)) {
            return _missilePowerUp;
        }
        if(t == typeof(TripleMachineGunPowerUp)) {
            return _multishotPowerUp;
        }

        return null;
    }
}