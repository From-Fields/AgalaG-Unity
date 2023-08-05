using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;

public class Player : MonoBehaviour, Entity
{
    // Attributes
    public bool isDead { get; private set; } = false;
    [SerializeField]
    private int _maxHealth = 3;
    private int _currentHealth;
    private bool _isInvulnerable;

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

#if UNITY_EDITOR
    public GameObject[] testWeapons;
#endif

    public List<iPowerUp> powerUps = new List<iPowerUp>();

    public int shields = 0;

    public UnityAction<int> onLifeUpdate;
    public UnityAction<int> onShieldUpdate;
    public UnityAction<Sprite, string> onNewWeapon;
    public UnityAction<string> onWeaponShoot;
    public UnityAction onDeath;

    // References
    private InputHandler _inputHandler => InputHandler.Instance;
    private Rigidbody2D _rigidbody;
    private AudioManager _audioManager;
    private SpriteRenderer _visuals;

    // Input Variables
    private Vector2 _movement = Vector2.zero;
    
    // Properties
    public int MaxHealth => _maxHealth;
    public AudioManager AudioManager => _audioManager;

    // Methods
    public void SwitchWeapon(Weapon newWeapon)
    {
        this.currentWeapon.DisposeWeapon();
        this.currentWeapon = newWeapon;
        newWeapon.transform.parent = transform;
        newWeapon.transform.position = transform.position;
        newWeapon.Initialize(LayerMask.NameToLayer("PlayerBullets"));
        newWeapon.onShoot += PlayWeaponSound;

        onNewWeapon?.Invoke(this.currentWeapon.WeaponIcon, this.currentWeapon.AmmunitionToString);
    }

    public void SwitchToDefaultWeapon() => SwitchWeapon(_defaultWeapon);

    public void AddPowerUp(iPowerUp newPowerUp) {
        newPowerUp.OnPickup(this);

        if(newPowerUp.IsInstant) {
            PlaySound(EntityAudioType.PowerUp);
            return;
        }

        if(!this.powerUps.Contains(newPowerUp)) {
            PlaySound(EntityAudioType.PowerUp);
            this.powerUps.Add(newPowerUp);
        }

        onShieldUpdate?.Invoke(shields);
    }

    public void RemovePowerUp(iPowerUp powerUp) {
        this.powerUps.Remove(powerUp);
    }

    public void Heal(int amount)  {
        this._currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        onLifeUpdate?.Invoke(this._currentHealth);
    }

    private void SetInvulnerability(bool invulnerable) {
        _isInvulnerable = invulnerable;
        if(invulnerable)
            StartCoroutine(WaitForInvulnerability(1.5f));

        Debug.Log(invulnerable);
    }

    private IEnumerator WaitForInvulnerability(float time) {
        float accumulator = 0;
        float currTime = Time.time;
        float targetTime = currTime + time;
        Color c;

        while(currTime < targetTime) {
            accumulator += Time.deltaTime;
            currTime += Time.deltaTime;

            if(accumulator > time / 12) {
                c = _visuals.color;
                float a = (c.a == 1) ? 0.5f : 1;

                _visuals.color = new Color(c.r, c.g, c.b, a);

                accumulator = 0;
            }


            yield return new WaitForEndOfFrame();
        }

        c = _visuals.color;
        _visuals.color = new Color(c.r, c.g, c.b, 1);
        SetInvulnerability(false);

        yield return null;
    }

    //Sound Methods
    public void PlaySound(AudioClip clip) => _audioManager.PlaySound(clip);
    private void PlaySound(EntityAudioType audioType) => _audioManager.PlaySound(audioType);
    private void StopSound(EntityAudioType audioType) => _audioManager.StopSound(audioType);
    private void PlayWeaponSound() => _audioManager.PlaySound(EntityAudioType.Shot);

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

    public void Shoot()
    {
        currentWeapon.Shoot();
        onWeaponShoot?.Invoke(currentWeapon.AmmunitionToString);
        if (currentWeapon.isEmpty())
        {
            SwitchToDefaultWeapon();
        }
    }

    public void TakeDamage(int damage) {
        if(_isInvulnerable)
            return;

        int _damage = damage;

        for (int i = 0; i < powerUps.Count; i++) {
            _damage = powerUps[i].OnTakeDamage(_damage, _currentHealth); 
        }

        onShieldUpdate.Invoke(shields);

        this._currentHealth = Mathf.Clamp(_currentHealth - _damage, 0, _maxHealth);
        onLifeUpdate?.Invoke(this._currentHealth);
        PlaySound(EntityAudioType.Damage);

        if(this._currentHealth <= 0)
            this.Die();

        SetInvulnerability(true);
    }
    public void Die() {
        bool die = true;    

        foreach(PowerUp powerUp in powerUps)
            die = powerUp.OnDeath();

        if(!die)
            return;

        PlaySound(EntityAudioType.Death);
        StopSound(EntityAudioType.Movement);

        _visuals.enabled = false;
        _rigidbody.simulated = false;

        _audioManager.WaitForAudioClipDone(() => gameObject.SetActive(false));

        this.onDeath?.Invoke();
        this.isDead = true;
    }

    // Unity Hooks
    private void Awake() {
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
        _audioManager = GetComponentInChildren<AudioManager>();
        _visuals = GetComponentInChildren<SpriteRenderer>();

        _isInvulnerable = false;
        currentSpeed = _defaultSpeed;
        currentAcceleration = _defaultAcceleration;
        currentWeapon = _defaultWeapon;
        _currentHealth = _maxHealth;
        SwitchToDefaultWeapon();
        _audioManager.PlaySound(EntityAudioType.Movement, looping: true);
    }
    private void Update() {
        if(isDead) 
            return;

        foreach(PowerUp powerUp in powerUps)
            powerUp.OnTick();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Weapon tw = Instantiate(testWeapons[0], gameObject.transform).GetComponent<Weapon>();
            SwitchWeapon(tw);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Weapon tw = Instantiate(testWeapons[1], gameObject.transform).GetComponent<Weapon>();
            SwitchWeapon(tw);
        }
#endif

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
