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
    protected int _currentAmmuntion;

    protected string _shooter;

    public int MaxAmmunition => _maxAmmunition;
    public int CurrentAmmunition => _currentAmmuntion;

    void Awake() {
        Initialize();
    }

    protected abstract void Initialize();
    public abstract void isEmpty();
    public abstract void Shoot();
}
