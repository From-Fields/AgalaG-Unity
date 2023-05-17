using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGeminiChild : Enemy<EnemyGeminiChild>
{
    //Attributes
    private float _positionOffset;
    [SerializeField]
    private float _velocityMultiplier = 20;
    private float _orbitingVelocity;
    [SerializeField]
    private DefaultWeapon _weaponPrefab;
    private DefaultWeapon _weapon;

    //Health
    private int _currentHealth;
    private int _maxHealth = 1;

    //References
    private Rigidbody2D _rigidbody;
    private EnemyGemini _parent;

    //Initialization Methods
    public void SetParent(EnemyGemini parent, float positionOffset, float orbitingVelocity) {
        this._parent = parent;
        this._positionOffset = positionOffset;
        this._orbitingVelocity = orbitingVelocity;
    }
    public void SetWeapon(float weaponCooldown) {
        this._weapon.SetAttributes(cooldown: weaponCooldown, direction: Vector2.down);
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
    public override void Shoot() {
        _weapon.Shoot();
    }
    public override void TakeDamage(int damage) {
        damage = Math.Clamp(damage, 0, 1);

        _currentHealth = Math.Clamp(_currentHealth - damage, 0, _maxHealth);

        if(_currentHealth == 0)
            Die();
    }

    //Enemy
    protected override void SubInitialize() {
        this._currentSpeed = _defaultSpeed;
        this._currentAcceleration = _defaultAcceleration;

        _isDead = false;
        _currentHealth = _maxHealth;
        _weapon = _weaponPrefab;
    }
    protected override void SubReserve() {
        this._parent.TakeDamage(1);
    }
    protected override void SubFixedUpdate() {
        Vector2 fromChild = (Position - _parent.Position).normalized;
        Vector2 toChild = (_parent.Position - Position).normalized;
        Vector2 tangent = (Quaternion.Euler(0, 0, 90) * fromChild).normalized;
        float offsetMultiplier = (_velocityMultiplier + (_positionOffset * 5 / 4));
        float acceleration = _parent.CurrentAcceleration;

        if(Vector2.Distance(Position, _parent.Position) > _positionOffset)
            Move(toChild, _orbitingVelocity * offsetMultiplier, acceleration);
        else
            Move(fromChild, _orbitingVelocity * offsetMultiplier, acceleration);

        Move(tangent, _orbitingVelocity * _velocityMultiplier, acceleration);

        Debug.Log(Vector2.Distance(Position, _parent.Position));

        this._fromChild = toChild;
        this._tangent = tangent;
    }

    private Vector2? _fromChild, _tangent;
    
    //iPoolableEntity
    public override EnemyGeminiChild OnCreate() => Instantiate<EnemyGeminiChild>(EntityPool<EnemyGeminiChild>.Instance.ObjReference);
    public override Action<EnemyGeminiChild> onGetFromPool => null;
    public override IObjectPool<EnemyGeminiChild> Pool => EntityPool<EnemyGeminiChild>.Instance.Pool;
    public override void Reserve() => Pool.Release(this);
    #endregion

    private void OnDrawGizmos() {
        if(_fromChild.HasValue) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + _fromChild.Value);
        }
        if(_tangent.HasValue) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Position, Position + _tangent.Value);
        }
    }
}
