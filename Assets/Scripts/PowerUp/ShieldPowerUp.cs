using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShieldPowerUp : PowerUp
{
    public override Sprite Sprite => PrefabRepository.Instance.GetPowerUpOfType(typeof(ShieldPowerUp));
    public AudioClip clip => PrefabRepository.Instance.GetAudioOfType(typeof(ShieldPowerUp));
    public override bool IsInstant => false;

    public override void OnPickup(Player player) {
        this._player = player;
        this._player.shields++;
    }

    public override int OnTakeDamage(int damage, int playerHealth) {
        if(damage <= 0)
            return damage;

        EndPowerUp();
        return damage - 1;
    }
    public override void OnEnd() {
        _player.PlaySound(clip);
        _player.RemovePowerUp(this);
        _player.shields--;
    }
}
