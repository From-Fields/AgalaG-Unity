using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBumblebee : Enemy<EnemyBumblebee>
{
    //Attributes
    [Header("Weapon")][SerializeField]
    private int _weaponDamage = 1;
    [SerializeField]
    private float _weaponCooldown = 0.5f;
    [SerializeField]
    private float _missileSpeed = 5f;
    [SerializeField]
    private DefaultWeapon _weaponReference;

    //Health
    [Header("Health")][SerializeField]
    private int _defaultHealth;
    private int _currentHealth;
    private int _maxHealth = 1;

    //References
    private Rigidbody2D _rigidbody;
    private Vector2 _desiredVelocity;

    public void SetWeapon(float weaponCooldown, int missileDamage, float missileSpeed) {
        this._weaponReference.SetAttributes(damage: missileDamage, cooldown: weaponCooldown, speed: missileSpeed, direction: Vector2.down);
    }

    //Unity Hooks
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    #region InterfaceImplementation
    //iEntity
    public override int health => _currentHealth;
    public override Vector2 Position => _rigidbody.position;
    public override Vector2 CurrentVelocity => _rigidbody.velocity;

    //iEnemy
    public override Rigidbody2D Rigidbody => _rigidbody;
    public override void Move(Vector2 direction, float speed, float acceleration) =>
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, direction * speed * Time.fixedDeltaTime, Time.fixedDeltaTime * acceleration);
    public override void Stop() =>
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, 0.99f);
    public override void Shoot() => _weaponReference.Shoot();
    public override void TakeDamage(int damage) {
        _currentHealth = Math.Clamp(_currentHealth - damage, 0, _maxHealth);

        if(_currentHealth == 0)
            Die();
    }

    //Enemy
    protected override void SubInitialize() {
        this._currentSpeed = _defaultSpeed;
        this._currentAcceleration = _defaultAcceleration;

        this._isDead = false;
        this._maxHealth = this._defaultHealth;
        this._currentHealth = this._defaultHealth;
        this._collisionDamage = this._defaultCollisionDamage;

        this.SetWeapon(_weaponCooldown, _weaponDamage, _missileSpeed);
    }
    private Vector2? _toChild, _fromChild, _tangent;
    
    //iPoolableEntity
    public override EnemyBumblebee OnCreate() => Instantiate<EnemyBumblebee>(EntityPool<EnemyBumblebee>.Instance.ObjReference);
    public override Action<EnemyBumblebee> onGetFromPool => null;
    public override IObjectPool<EnemyBumblebee> Pool => EntityPool<EnemyBumblebee>.Instance.Pool;
    public override void Reserve() => Pool.Release(this);
    #endregion

    private void OnDrawGizmos() {
        if(_toChild.HasValue) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + _toChild.Value);
        }
        if(_tangent.HasValue) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Position, Position + _tangent.Value);
        }
    }
}
