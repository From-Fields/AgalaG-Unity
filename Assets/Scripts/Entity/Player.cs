using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Entity
{
    // Attributes
    public bool isDead { get; private set; } = false;
    [SerializeField]
    private int _maxHealth = 3;
    private int _currentHealth;

    [HideInInspector]
    public Weapon currentWeapon;
    [SerializeField]
    private Weapon _defaultWeapon;

    private Vector2 _currentVelocity = Vector2.zero;

    [HideInInspector]
    public float currentSpeed;
    [SerializeField] [Range(10, 100)]
    private float _defaultSpeed;
    public List<PowerUp> powerUps;

    // References
    private InputHandler _inputHandler => InputHandler.Instance;
    private Rigidbody2D _rigidbody;

    // Methods
    public void SwitchWeapon(Weapon newWeapon) {
        this.currentWeapon = newWeapon;
    }
    public void AddPowerUp(PowerUp newPowerUp) {
        if(!this.powerUps.Contains(newPowerUp)) {
            this.powerUps.Add(newPowerUp);
        }
    }
    public void RemovePowerUp(PowerUp powerUp) {
        this.powerUps.Remove(powerUp);
    }
    public void Heal(int amount) {
        this._currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
    }

    // Entity Implementation    
    public int health => this._currentHealth;
    public Vector2 CurrentVelocity => this._currentVelocity;
    public Vector2 Position => this._rigidbody.position;

    public void Move(Vector2 direction, float speed) {
        Vector2 nDirection = direction.normalized;
        _currentVelocity = nDirection;

        Vector2 destination = transform.position + (Vector3) nDirection * speed;

        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
    }
    public void Shoot() {
        Debug.Log("Pew");
    }

    public void TakeDamage(int damage) {
        this._currentHealth -= damage;

        if(this._currentHealth <= 0)
            this.Die();
    }
    public void Die() {
        Debug.Log("NANI");
        this.isDead = true;
    }

    // Unity Hooks
    private void Awake() {
        _rigidbody = GetComponentInChildren<Rigidbody2D>();

        currentSpeed = _defaultSpeed;
        currentWeapon = _defaultWeapon;
        _currentHealth = _maxHealth;
    }
    private void Update() {
        if(!isDead) {
            if(_inputHandler.HasMovement) {
                Move(_inputHandler.Movement, currentSpeed);
            }
            if(_inputHandler.Shoot) {
                Shoot();
            }
        }
    }
}
