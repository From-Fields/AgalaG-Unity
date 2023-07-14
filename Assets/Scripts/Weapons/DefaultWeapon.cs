using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultWeapon : Weapon
{
    private Vector2 _direction = Vector2.up;

    protected override void Initialize() {
        _shooter = gameObject.tag;
    }

    // A arma padrao nao tera limite de tiros, entao nao fara nada em isEmpty
    public override bool isEmpty() 
    {
        return false;
    }

    public override void Shoot()
    {
        if(!_canShoot)
            return;

        StartCooldown();
        
        bullet = Instantiate(bulletPrefab, spawnPoint[0].transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.gameObject.layer = (_layer != -1) ? _layer : gameObject.layer;
        bullet.Initialize(_direction, _speed, _shooter, damage: _damage);
    }

    public void SetAttributes(Vector2? direction = null, int maxAmmunition = -1, float speed = -1, float cooldown = -1, int damage = -1, int layer = -1) {
        this._direction = direction.HasValue ? direction.Value : Vector2.up;
        this._maxAmmunition = (maxAmmunition != -1) ? maxAmmunition : _maxAmmunition;
        this._speed = (speed != -1) ? speed : _speed;
        this._cooldown = (cooldown != -1) ? cooldown : _cooldown;
        this._damage = (damage != -1)? damage : _damage;
        this._layer = layer;
    }
}
