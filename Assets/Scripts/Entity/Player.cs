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

    [HideInInspector]
    public float currentSpeed;
    [SerializeField] [Range(1, 100)]
    private float _defaultSpeed;

    [HideInInspector]
    public float currentAcceleration;
    [SerializeField] [Range(0, 100)]
    private float _defaultAcceleration = 10;
    public List<iPowerUp> powerUps = new List<iPowerUp>();

    public System.Action onDeath;

    // References
    private InputHandler _inputHandler => InputHandler.Instance;
    private Rigidbody2D _rigidbody;

    // Input Variables
    private Vector2 _movement = Vector2.zero;
    
    // Properties
    public int MaxHealth => _maxHealth;

    // Methods
    public void SwitchWeapon(Weapon newWeapon) => this.currentWeapon = newWeapon;
    public void SwitchToDefaultWeapon() => SwitchWeapon(_defaultWeapon);

    public void AddPowerUp(iPowerUp newPowerUp) {
        newPowerUp.OnPickup(this);

        if(newPowerUp.IsInstant)
            return;

        if(!this.powerUps.Contains(newPowerUp)) {
            this.powerUps.Add(newPowerUp);
        }
    }
    public void RemovePowerUp(iPowerUp powerUp) {
        this.powerUps.Remove(powerUp);
    }
    public void Heal(int amount) => this._currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);

    // Entity Implementation    
    public int health => this._currentHealth;
    public Vector2 CurrentVelocity => _rigidbody.velocity;
    public Vector2 Position => this._rigidbody.position;

    public void Move(Vector2 direction, float speed, float acceleration) {
        Vector2 nDirection = direction.normalized;

        Vector2 destination = nDirection * speed;

        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, destination, Time.fixedDeltaTime * acceleration);
    }
    public void Stop() => _rigidbody.velocity = Vector2.zero;
    public void Shoot() => currentWeapon.Shoot();
    public void TakeDamage(int damage) {
        int _damage = damage;

        for (int i = 0; i < powerUps.Count; i++) {
            _damage = powerUps[i].OnTakeDamage(_damage, _currentHealth); 
        }

        Debug.Log(_damage);

        this._currentHealth -= _damage;

        if(this._currentHealth <= 0)
            this.Die();
    }
    public void Die() {
        bool die = true;    

        foreach(PowerUp powerUp in powerUps)
            die = powerUp.OnDeath();

        if(!die)
            return;

        gameObject.SetActive(false);
        this.onDeath?.Invoke();
        this.isDead = true;
    }

    // Unity Hooks
    private void Awake() {
        _rigidbody = GetComponentInChildren<Rigidbody2D>();

        currentSpeed = _defaultSpeed;
        currentAcceleration = _defaultAcceleration;
        currentWeapon = _defaultWeapon;
        _currentHealth = _maxHealth;
    }
    private void Update() {
        if(isDead) 
            return;

        foreach(PowerUp powerUp in powerUps)
            powerUp.OnTick();

        _movement = (_inputHandler.HasMovement) ? _inputHandler.Movement : Vector2.zero;
        if(_inputHandler.Shoot)
            Shoot();
    }
    private void FixedUpdate()
    {
        if(isDead)
            return;

        Move(_movement, currentSpeed, currentAcceleration);
    }
}
