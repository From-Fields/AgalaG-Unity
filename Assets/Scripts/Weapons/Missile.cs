using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon
{
    private Vector2 _direction = Vector2.up;

    [Space]
    [SerializeField] private GameObject explosion;

    private Coroutine _reloadCoroutine = null;

    public override Sprite WeaponIcon => Resources.Load<Sprite>("Sprites/Bullet/Missile_Player");

    public override void Initialize(LayerMask layer)
    {
        _shooter = transform.parent.gameObject.tag;
        _layer = layer;
    }

    public override void Shoot()
    {
        if (!_canShoot || _currentAmmuntion <= 0)
            return;

        _reloadCoroutine = StartCooldown();

        bullet = Instantiate(bulletPrefab, spawnPoint[0].transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.gameObject.layer = (_layer != -1) ? _layer : gameObject.layer;
        bullet.Initialize(_direction, _speed, _shooter, damage: _damage, explosion: true);
        _currentAmmuntion--;
        onShoot?.Invoke();
    }

    protected override void SubDisposeWeapon()
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
