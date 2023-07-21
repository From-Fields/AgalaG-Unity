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

    private AudioManager _audioManager;

    private Vector3 _position = Vector3.zero;

    public void Initialize(
        PowerUp powerUp, Vector2 position, Vector2 direction, 
        Bounds levelBounds, float speed = 5, 
        bool rotate = true, float rotationSpeed = 100f, 
        bool doScale = true, float maximumScale = 1.3f, float scaleSpeed = 5f
    ) {
        gameObject.SetActive(true);
        this._powerUp = powerUp;

        _rigidbody.position = GetStartingPosition(position, levelBounds);
        transform.position = GetStartingPosition(position, levelBounds);

        ApplyMovement(direction, speed);
        this._visuals.Initialize(powerUp.Sprite, rotate, rotationSpeed, doScale, maximumScale, scaleSpeed);
    }

    private Vector2 GetStartingPosition(Vector3 position, Bounds levelBounds) {
        if(levelBounds.Contains(position))
            return position;

        Vector3 colliderSize = _collider.bounds.extents;

        float minX = levelBounds.center.x - levelBounds.extents.x + (colliderSize.x * 2);
        float maxX = levelBounds.center.x + levelBounds.extents.x - (colliderSize.x * 2);
        float minY = levelBounds.center.y - levelBounds.extents.y + (colliderSize.y * 2);
        float maxY = levelBounds.center.y + levelBounds.extents.y - (colliderSize.y * 2);

        position.x = (position.x < minX ) ? minX 
            : (position.x > maxX) ? maxX
                : position.x;

        position.y = (position.y < minY ) ? minY 
            : (position.y > maxY) ? maxY
                : position.y; 

        _position = position;

        return position;
    }

    // Movement Methods
    private void ApplyMovement(Vector2 direction, float speed) => _rigidbody.velocity = direction * speed;
    private void ReflectMovement(Collider2D other) {
        Vector2 contact = other.ClosestPoint(_rigidbody.position);
        Vector2 velocity = _rigidbody.velocity;
        Vector2 normal = (Vector2) transform.position - contact;
        normal.Normalize();

        Vector2 targetVelocity = velocity - 2 * (Vector2.Dot(velocity, normal) * normal);
        _audioManager.PlaySound(EntityAudioType.Bounce);

        _rigidbody.velocity = targetVelocity;
    }

    // Unity Hooks
    private void Awake() {
        gameObject.layer = LayerMask.NameToLayer("PickUps");
        _collider = gameObject.GetComponent<Collider2D>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _visuals = GetComponentInChildren<PickUpVisual>();
        _audioManager = GetComponentInChildren<AudioManager>();

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

        Pool.Release(this);
        gameObject.SetActive(false);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(_position, 0.3f);
    }
}
