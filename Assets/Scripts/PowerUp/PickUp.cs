using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PickUp : MonoBehaviour, iPoolableObject<PickUp>
{
    [SerializeField]
    private float _initialSpeed;
    [SerializeField]
    private Vector2 _initialDirection = Vector2.zero;

    // References
    private PowerUp _powerUp;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private PickUpVisual _visuals;

    public void Initialize(
        PowerUp powerUp, Vector2 position, Vector2 direction, float speed = 5, 
        bool rotate = true, float rotationSpeed = 100f, 
        bool doScale = true, float maximumScale = 1.3f, float scaleSpeed = 5f
    ) {
        this._powerUp = powerUp;
        transform.position = position;
        ApplyMovement(direction, speed);
        this._visuals.Initialize(powerUp.Sprite, rotate, rotationSpeed, doScale, maximumScale, scaleSpeed);
    }

    // Movement Methods
    private void ApplyMovement(Vector2 direction, float speed) => _rigidbody.velocity = direction * speed;
    private void ReflectMovement(Collider2D other) {
        Vector2 contact = other.ClosestPoint(_rigidbody.position);
        Vector2 velocity = _rigidbody.velocity;
        Vector2 normal = (Vector2) transform.position - contact;
        normal.Normalize();

        Vector2 targetVelocity = velocity - 2 * (Vector2.Dot(velocity, normal) * normal);

        _rigidbody.velocity = targetVelocity;
    }

    // Unity Hooks
    private void Awake() {
        gameObject.layer = LayerMask.NameToLayer("PickUps");
        _collider = gameObject.GetComponent<Collider2D>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _visuals = GetComponentInChildren<PickUpVisual>();

        #if UNITY_EDITOR
            if(debug) {
                ApplyMovement(_initialDirection.normalized, _initialSpeed);
                AddPowerUp();
            }
        #endif
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();

        if(player == null) {
            ReflectMovement(collision);
            return;
        }

        player.AddPowerUp(_powerUp);

        gameObject.SetActive(false);
        Pool.Release(this);
    }

    private void OnValidate() {
        if(GetComponentInChildren<PickUpVisual>() == null) {
            GameObject obj = new GameObject("visuals", typeof(PickUpVisual));

            obj.transform.SetParent(transform);
        }
    }

    // PoolableObject Implementation
    public PickUp OnCreate() => Instantiate<PickUp>(SingletonObjectPool<PickUp>.Instance.ObjReference);
    public Action<PickUp> onGetFromPool => null;
    public Action<PickUp> onReleaseToPool => null;
    public IObjectPool<PickUp> Pool => SingletonObjectPool<PickUp>.Instance.Pool;
    
    #if UNITY_EDITOR
        [SerializeField]
        private bool debug = false;
        [SerializeField]
        private PowerUpType _powerUpType;

        public enum PowerUpType {
            Shield,
            Heal,
            Weapon
        }

        private void AddPowerUp() {
            if(_powerUpType == PowerUpType.Shield)
                _powerUp = new ShieldPowerUp();
            if(_powerUpType == PowerUpType.Heal)
                _powerUp = new RepairPowerUp();
        }
    #endif
}
