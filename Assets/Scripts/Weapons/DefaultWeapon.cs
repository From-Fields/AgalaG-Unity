using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : Weapon
{
    protected override void Initialize() {
        _shooter = gameObject.tag;
    }

    // A arma padrão não terá limite de tiros, então não fará nada em isEmpty
    public override void isEmpty() { }

    public override void Shoot()
    {
        bullet = Instantiate(bulletPrefab, spawnPoint[0].transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Initialize(Vector2.up, _speed, _shooter);
    }

}
