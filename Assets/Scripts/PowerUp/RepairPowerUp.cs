using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPowerUp : PowerUp
{
    public override bool IsInstant => true;

    public override void OnPickup(Player player) {
        player.Heal(player.MaxHealth);
    }
    public override void OnEnd() { }  // Do Nothing
}
