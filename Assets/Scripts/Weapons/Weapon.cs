using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    protected Bullet bullet;
    
    [Space]
    [SerializeField] protected Transform[] spawnPoint = new Transform[1];
    [SerializeField] protected int _maxAmmunition = 1;
    [SerializeField] [Min(0.01f)] protected float _speed = 2f;
    [SerializeField] [Min(0.01f)] protected float _cooldown = 0.1f;
    protected int _currentAmmuntion;

    [Space]
    [SerializeField][Min(1)] protected int _damage = 1;
    [SerializeField] protected int _layer = -1;

    protected string _shooter;
    protected bool _canShoot = true;

    public Action onShoot;

    public int MaxAmmunition => _maxAmmunition;
    public int CurrentAmmunition => _currentAmmuntion;

    public virtual string AmmunitionToString => CurrentAmmunition.ToString();

    public virtual Sprite WeaponIcon => Resources.Load<Sprite>("Sprites/Bullet/Bullet_Player");

    void Awake() {
        _currentAmmuntion = _maxAmmunition;
        _canShoot = true;
    }


    protected Coroutine StartCooldown()
    {
        if (_cooldown <= 0)
            return null;

        _canShoot = false;
        return CoroutineRunner.Instance.CallbackTimer(this._cooldown, OnCooldownEnd);
    }

    protected void OnCooldownEnd() => _canShoot = true;

    public void DisposeWeapon() {
        onShoot = null;
        SubDisposeWeapon();
    }
    protected abstract void SubDisposeWeapon();

    public abstract void Initialize(LayerMask layer);
    public abstract bool isEmpty();
    public abstract void Shoot();
}
