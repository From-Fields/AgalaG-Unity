using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKamikaze: Enemy<EnemyKamikaze>
{
    //Attributes
    //Health
    [Header("Health")][SerializeField]
    private int _defaultHealth = 1;
    private int _currentHealth;
    private int _maxHealth;

    //References
    [SerializeField]
    private GameObject target;
    private Rigidbody2D _rigidbody;

    //Properties
    public override Rigidbody2D Rigidbody => _rigidbody;

    // Unity Hooks
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    #region InterfaceImplementation
    //iEntity
    public override int health => _currentHealth;
    public override Vector2 CurrentVelocity => _rigidbody.velocity;
    public override Vector2 Position => _rigidbody.position;

    public override void Move(Vector2 direction, float speed, float acceleration) =>
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, direction * speed * Time.fixedDeltaTime, Time.fixedDeltaTime * acceleration);
    public override void Stop() =>
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, 0.99f);
    public override void Shoot() { }
    public override void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        if(_currentHealth == 0)
            Die();
    }

    //iPoolableEntity
    public override EnemyKamikaze OnCreate() => Instantiate<EnemyKamikaze>(EntityPool<EnemyKamikaze>.Instance.ObjReference);
    public override Action<EnemyKamikaze> onGetFromPool => null;
    public override IObjectPool<EnemyKamikaze> Pool => EntityPool<EnemyKamikaze>.Instance.Pool;

    //Enemy
    protected override void SubInitialize() 
    {
        this._currentSpeed = _defaultSpeed;
        this._currentAcceleration = _defaultAcceleration;

        this._isDead = false;
        this._maxHealth = this._defaultHealth;
        this._currentHealth = this._defaultHealth;
    }
    public override void Reserve() => Pool.Release(this);
    #endregion    
}
