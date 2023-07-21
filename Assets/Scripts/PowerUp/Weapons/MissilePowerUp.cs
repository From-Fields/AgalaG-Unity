using System;
using System.Collections.Generic;
using UnityEngine;

public class MissilePowerUp: WeaponPowerUp
{
    public override Sprite Sprite => PrefabRepository.Instance.GetPowerUpOfType(typeof(MissilePowerUp));

    protected override Weapon GetWeapon() => MonoBehaviour.Instantiate<Missile>(PrefabRepository.Instance.GetPrefabOfType(typeof(Missile)).GetComponent<Missile>());
}