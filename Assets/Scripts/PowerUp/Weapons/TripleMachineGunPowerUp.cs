using System;
using System.Collections.Generic;
using UnityEngine;

public class TripleMachineGunPowerUp: WeaponPowerUp
{
    public override Sprite Sprite => PrefabRepository.Instance.GetPowerUpOfType(typeof(TripleMachineGunPowerUp));

    protected override Weapon GetWeapon() => 
        MonoBehaviour.Instantiate<TripleMachineGun>(
            PrefabRepository.Instance.GetPrefabOfType(typeof(TripleMachineGun)).GetComponent<TripleMachineGun>()
        );
}