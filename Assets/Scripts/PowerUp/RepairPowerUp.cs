using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RepairPowerUp : PowerUp
{
    public override Sprite Sprite => PrefabRepository.Instance.GetPowerUpOfType(typeof(RepairPowerUp));
    public override bool IsInstant => true;

    public override void OnPickup(Player player) {
        player.Heal(player.MaxHealth);
    }
    public override void OnEnd() { }  // Do Nothing
}
