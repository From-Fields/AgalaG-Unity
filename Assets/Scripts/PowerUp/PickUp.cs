using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PickUp : MonoBehaviour
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
        PowerUp powerUp, Vector2 direction, float speed, Sprite sprite, 
        bool rotate = false, float rotationSpeed = 100f, 
        bool doScale = false, float maximumScale = 1.3f, float scaleSpeed = 5f
    ) {
        this._powerUp = powerUp;
        ApplyMovement(direction, speed);
        this._visuals.Initialize(sprite, rotate, rotationSpeed, doScale, maximumScale, scaleSpeed);
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
        Destroy(this, 0.1f);
    }

    private void OnValidate() {
        if(GetComponentInChildren<PickUpVisual>() == null) {
            GameObject obj = new GameObject("visuals", typeof(PickUpVisual));

            obj.transform.SetParent(transform);
        }
    }
    
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
