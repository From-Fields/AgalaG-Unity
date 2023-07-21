using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Hazard: MonoBehaviour, Entity, iPoolableEntity<Hazard>
{
    [SerializeField]
    private Sprite _sprite;
    private uint _maxBounces = 0, _currentBounces = 0, _damage = 1, _health = 1;
    private bool _rotate, _canBounce, _isWithinBounds;
    private float _rotationSpeed;
    private Vector2 _initialDirection = Vector2.zero;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private AudioManager _audioManager;

    private PickUpVisual _visuals;

    public void Initialize(
        Vector2 position, Vector2 direction, Sprite sprite = null,
        float speed = 5, uint damage = 1, uint health = 1,
        bool rotate = true, float rotationSpeed = 100,  Vector2? scale = null,
        uint maxBounces = 0
    ) {
        transform.position = position;
        gameObject.SetActive(true);
        
        this._isWithinBounds = false;
        this._canBounce = (maxBounces != 0);
        this._maxBounces = maxBounces;
        this._currentBounces = 0;
        
        this._visuals.Initialize(sprite ?? _sprite, rotate, rotationSpeed, baseScale: scale);

        this._damage = damage;
        this._health = health;

        Move(direction, speed, 0f);
    }

    // Movement Methods
    private bool WillBounce() => (_canBounce && _isWithinBounds && _currentBounces < _maxBounces);
    private void ReflectMovement(Collider2D other) 
    {
        Vector2 contact = other.ClosestPoint(_rigidbody.position);
        Vector2 velocity = _rigidbody.velocity;
        Vector2 normal = (Vector2) transform.position - contact;
        normal.Normalize();

        Vector2 targetVelocity = velocity - 2 * (Vector2.Dot(velocity, normal) * normal);
        _audioManager.PlaySound(EntityAudioType.Bounce);

        _rigidbody.velocity = targetVelocity;
        this._currentBounces++;
    }

    // Unity Hooks
    private void Awake() {
        gameObject.layer = LayerMask.NameToLayer("Hazards");
        _collider = gameObject.GetComponent<Collider2D>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _visuals = GetComponentInChildren<PickUpVisual>();
        _audioManager = GetComponentInChildren<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();

        if(player == null) {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Walls")) {
                if(WillBounce())
                    ReflectMovement(collision);
                else if(_isWithinBounds) {
                    CoroutineRunner.Instance.CallbackTimer(1.5f, ReserveToPool);
                }
            }

            return;
        }
        player.TakeDamage((int) _damage);
        ReserveToPool();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(_isWithinBounds)
            return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Walls")) {
            this._isWithinBounds = true;
        }
    }

    private void OnValidate() {
        if(GetComponentInChildren<PickUpVisual>() == null) {
            GameObject obj = new GameObject("visuals", typeof(PickUpVisual));

            obj.transform.SetParent(transform);
        }
    }

    // PoolableObject Implementation
    public Hazard OnCreate() => Instantiate(SingletonObjectPool<Hazard>.Instance.ObjReference);
    public Action<Hazard> onGetFromPool => null;
    public Action<Hazard> onReleaseToPool => null;
    public IObjectPool<Hazard> Pool => SingletonObjectPool<Hazard>.Instance.Pool;

    public void ReserveToPool() 
    {
        _visuals.SetEnabled(true);
        _rigidbody.simulated = true;

        if(gameObject.activeSelf == false)
            return;

        gameObject.SetActive(false);
        Pool.Release(this);
    }

    public int health => (int) this._health;
    public Vector2 Position => transform.position;
    public Vector2 CurrentVelocity => _rigidbody.velocity;
    public void Move(Vector2 direction, float speed, float acceleration) => _rigidbody.velocity = direction * speed;
    public void Shoot() {  } //Do Nothing
    public void Stop() => _rigidbody.velocity = Vector2.zero;

    public void TakeDamage(int damage)
    {
        _health = (uint) Math.Clamp(_health - damage, 0, _health);

        if(_health <= 0)
            Die();
    }
    public void Die() {
        _audioManager.PlaySound(EntityAudioType.Death);

        _visuals.SetEnabled(false);
        _rigidbody.simulated = false;

        _audioManager.WaitForAudioClipDone(ReserveToPool);
    }
}