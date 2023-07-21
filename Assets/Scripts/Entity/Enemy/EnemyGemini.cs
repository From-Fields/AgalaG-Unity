using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGemini : Enemy<EnemyGemini>
{
    //Attributes
    [Header("Child Settings")][SerializeField]
    private float _geminiPositionOffset = 0.5f;
    [SerializeField]
    private float _orbitingVelocity = 1f;
    [SerializeField]
    private int _geminiMissileDamage = 1;
    [SerializeField]
    private float _weaponCooldown = 1f;
    [SerializeField]
    private float _missileSpeed = 4f;

    //Health
    [Header("Health")][SerializeField]
    private int _defaultHealth = 2;
    private int _currentHealth;
    private int _maxHealth;

    //References
    private List<EnemyGeminiChild> _children = new List<EnemyGeminiChild>();
    private Rigidbody2D _rigidbody;
    public IObjectPool<EnemyGeminiChild> _childPool => EntityPool<EnemyGeminiChild>.Instance.Pool;

    internal void PlayDeathSound() => _audioManager.PlaySound(EntityAudioType.Death);
    
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
    public override void Move(Vector2 direction, float speed, float acceleration) {
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, direction * speed * Time.fixedDeltaTime, Time.fixedDeltaTime * acceleration);
        foreach(var child in _children) {
            child.Move(direction, speed, acceleration);
        }
    }
    public override void Stop() =>
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, 0.99f);
    public override void Shoot() {
        foreach (var child in _children) {
            child.Shoot();
        }
    }
    public override void TakeDamage(int damage) {
        damage = Math.Clamp(damage, 0, 1);

        _currentHealth = Math.Clamp(_currentHealth - damage, 0, _maxHealth);

        if(_currentHealth == 0)
            Die();
    }
    protected override void OnCollision(Collision2D other) { /*Do Nothing*/}
    
    //iPoolableEntity
    public override EnemyGemini OnCreate() => Instantiate<EnemyGemini>(EntityPool<EnemyGemini>.Instance.ObjReference);
    public override IObjectPool<EnemyGemini> Pool => EntityPool<EnemyGemini>.Instance.Pool;

    protected override void SubInitialize() {
        this._currentSpeed = _defaultSpeed;
        this._currentAcceleration = _defaultAcceleration;

        this._isDead = false;
        this._maxHealth = this._defaultHealth;
        this._currentHealth = this._defaultHealth;

        for (int i = 0; i < _defaultHealth; i++) {
            this._children.Add(_childPool.Get());
            var child = this._children[i];
            float yOffset = (i < 1) ? -1 * this._geminiPositionOffset : this._geminiPositionOffset;
            Vector2 position = new Vector2(this.Position.x, this.Position.y + yOffset);

            child.Initialize(new Queue<iEnemyAction>(), null, new WaitSeconds(200), position, _levelBounds);
            child.SetParent(this, _geminiPositionOffset, _orbitingVelocity);
            child.SetWeapon(_weaponCooldown, _geminiMissileDamage, _missileSpeed);
        }
    }
    protected override void ReserveToPool() => Pool.Release(this);
    protected override void SubReserve() {
        base.SubReserve();

        int childCount = _children.Count;

        for (int i = 0; i < childCount; i++) {

            EnemyGeminiChild child = _children[i];

            if(!child.IsDead)
                child.Reserve();
        }

        _children.Clear();
    }

    #endregion
}
