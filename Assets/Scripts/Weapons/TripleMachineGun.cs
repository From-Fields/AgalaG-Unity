using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleMachineGun : Weapon
{
    private Vector2 _direction = Vector2.up;

    private Coroutine _reloadCoroutine = null;

    protected override void Initialize()
    {
        _shooter = transform.parent.tag;
    }

    public override void Shoot()
    {
        if (!_canShoot)
            return;

        _reloadCoroutine = StartCooldown();

        for (int i = 0; i < 3; i++)
        {
            bullet = Instantiate(bulletPrefab, spawnPoint[i].transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.gameObject.layer = (_layer != -1) ? _layer : gameObject.layer;
            bullet.Initialize(_direction, _speed, _shooter, damage: _damage);
        }
        _currentAmmuntion--;
    }

    public override void DisposeWeapon()
    {
        CoroutineRunner.Instance.CancelCallback(_reloadCoroutine);
        Destroy(gameObject);
    }

    public override bool isEmpty()
    {
        if (_currentAmmuntion <= 0)
        {
            CoroutineRunner.Instance.CancelCallback(_reloadCoroutine);
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
