using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : Weapon
{
    private bool _canShoot = true;
    private Vector2 _direction = Vector2.up;

    protected override void Initialize() {
        _shooter = gameObject.tag;
        _canShoot = true;
    }

    // A arma padr�o n�o ter� limite de tiros, ent�o n�o far� nada em isEmpty
    public override void isEmpty() { }

    public override void Shoot()
    {
        if(!_canShoot)
            return;

        StartCooldown();
        
        bullet = Instantiate(bulletPrefab, spawnPoint[0].transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Initialize(_direction, _speed, _shooter);
    }

    private void StartCooldown()  {
        if(_cooldown <= 0)
            return;

        _canShoot = false;
        CoroutineRunner.Instance.CallbackTimer(this._cooldown, OnCooldownEnd);
    }
    private void OnCooldownEnd() => _canShoot = true;

    public void SetAttributes(Vector2? direction = null, int maxAmmunition = -1, float speed = -1, float cooldown = -1) {
        this._direction = direction.HasValue ? direction.Value : Vector2.up;
        this._maxAmmunition = (maxAmmunition != -1) ? maxAmmunition : _maxAmmunition;
        this._speed = (speed != -1) ? speed : _speed;
        this._cooldown = (cooldown != -1) ? cooldown : _cooldown;
    }
}
