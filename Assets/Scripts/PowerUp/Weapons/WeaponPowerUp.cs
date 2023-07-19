using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponPowerUp :  PowerUp {
    private Sprite _sprite;

    public override Sprite Sprite => _sprite;
    protected abstract Weapon GetWeapon();
    public override bool IsInstant => true;

    public override void OnPickup(Player player) 
    {
        Weapon weapon = GetWeapon();
        player.SwitchWeapon(weapon);
    }
    public override void OnEnd() { }  // Do Nothing
}
