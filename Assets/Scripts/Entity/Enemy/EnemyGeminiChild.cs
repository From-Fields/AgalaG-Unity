using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGeminiChild : Enemy<EnemyGeminiChild>
{
    //Attributes
    private bool _wasKilled;
    private float _positionOffset;
    [SerializeField]
    private float _velocityMultiplier = 20;
    private float _orbitingVelocity;
    [Header("Weapon")][SerializeField]
    private DefaultWeapon _weapon;

    //Health
    private int _currentHealth;
    private int _maxHealth = 1;

    //References
    private Rigidbody2D _rigidbody;
    private EnemyGemini _parent;
    private Vector2? _desiredVelocity;

    //Initialization Methods
    public void SetParent(EnemyGemini parent, float positionOffset, float orbitingVelocity) {
        this._parent = parent;
        this._positionOffset = positionOffset;
        this._orbitingVelocity = orbitingVelocity;
    }
    public void SetWeapon(float weaponCooldown, int missileDamage, float missileSpeed) {
        this._weapon.SetAttributes(damage: missileDamage, cooldown: weaponCooldown, speed: missileSpeed, direction: Vector2.down);
        this._weapon.onShoot += PlayShotSound;
    }
    private void PlayShotSound() => _audioManager.PlaySound(EntityAudioType.Shot);

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
        _desiredVelocity = direction * speed;
    public override void Stop() {
        _desiredVelocity = Vector2.zero;
    }
    public override void Shoot() {
        if(_isDead)
            return;

        float currentOffset = Position.x - _parent.Position.x;
        // Debug.Log("Shoot!");
        
        if(Math.Abs(currentOffset) >= (0.9f * _positionOffset))
            _weapon.Shoot();
    }
    public override void TakeDamage(int damage) {
        _currentHealth = Math.Clamp(_currentHealth - damage, 0, _maxHealth);

        if(_currentHealth == 0) {
            Die();
        }
    }

    //Enemy
    protected override void SubInitialize() {
        this._currentSpeed = _defaultSpeed;
        this._currentAcceleration = _defaultAcceleration;

        this._isDead = false;
        this._wasKilled = false;
        this._currentHealth = this._maxHealth;
        this._collisionDamage = this._defaultCollisionDamage;
        this._droppedItem = null;

        this.onDeath += (_) => this._wasKilled = true;
        this.onDeath += (_) => OnDeath();
    }
    protected void OnDeath() {
        if(_wasKilled) {
            this._parent.TakeDamage(1);
            this._parent.PlayDeathSound();
            giveScore = true;
        }
    }
    protected override void SubUpdate() {
        Vector2 fromChild = (Position - _parent.Position).normalized;
        Vector2 toChild = (_parent.Position - Position).normalized;
        Vector2 tangent = (Quaternion.Euler(0, 0, 90) * fromChild).normalized;

        this._toChild = toChild;
        this._fromChild = fromChild;
        this._tangent = tangent;
    }
    protected override void SubFixedUpdate() {
        if(!_toChild.HasValue || !_fromChild.HasValue || !_tangent.HasValue)
            return;
        float distance = Vector2.Distance(Position, _parent.Position);
        float offsetMultiplier = (_velocityMultiplier * distance/_positionOffset);
        float acceleration = _parent.CurrentAcceleration;

        Vector2 _desiredOrbit = _tangent.Value * _orbitingVelocity * _velocityMultiplier * 2 * MathF.PI * _positionOffset / distance;

        // if(distance > _positionOffset)
            _desiredOrbit += (_toChild.Value * _orbitingVelocity * offsetMultiplier);
        // else
        //     _desiredOrbit += (_fromChild.Value * _orbitingVelocity * offsetMultiplier);

        if(_desiredVelocity.HasValue) {
            if(_desiredVelocity == Vector2.zero)
                _desiredVelocity = null;
            else {
                _desiredOrbit += _desiredVelocity.Value;
                _desiredVelocity = Vector2.zero;
            }
        }

        _desiredOrbit *= Time.fixedDeltaTime;

        //Debug.Log(_rigidbody.velocity);

        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, _desiredOrbit, Time.fixedDeltaTime * acceleration);
    }

    private Vector2? _toChild, _fromChild, _tangent;
    
    //iPoolableEntity
    public override EnemyGeminiChild OnCreate() => Instantiate<EnemyGeminiChild>(EntityPool<EnemyGeminiChild>.Instance.ObjReference);
    public override IObjectPool<EnemyGeminiChild> Pool => EntityPool<EnemyGeminiChild>.Instance.Pool;
    protected override void ReserveToPool() => Pool.Release(this);
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
