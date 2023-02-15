using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikaze : Enemy
{
    Rigidbody2D _rigidbody;

    [SerializeField]
    private int _defaultKamikazeDamage = 1;
    [SerializeField]
    private int _defaultHealth = 1;

    private int _currentHealth;
    private int _maxHealth;

    private int _kamikazeDamage;

    [SerializeField]
    GameObject target;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Queue<iEnemyAction> queue = new Queue<iEnemyAction>();
        queue.Enqueue(new MoveTowards(1, 1, 0.05f, 20, 0.1f, new Vector2(-1, 3)));
        queue.Enqueue(new Shoot(2));
        queue.Enqueue(new MoveTowards(5, 100, 0.05f, 40, 1f, target.GetComponentInChildren<Entity>()));

        Initialize(queue, new WaitSeconds(1), new WaitSeconds(1), this.Position);
    }

    public override Rigidbody2D Rigidbody => _rigidbody;
    public override int health => 0;

    public override Vector2 CurrentVelocity => _rigidbody.velocity;

    public override Vector2 Position => _rigidbody.position;

    public override void Die()
    {
        Reserve();
    }

    public override void Move(Vector2 direction, float speed, float acceleration)
    {
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, direction * speed * Time.fixedDeltaTime, Time.fixedDeltaTime * acceleration);
    }

    public override void Shoot()
    {
        Debug.Log("FIRING MAH LAZOR");
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        if(_currentHealth == 0)
            Die();
    }

    protected override void SubInitialize() 
    {
        this._currentSpeed = _defaultSpeed;
        this._currentAcceleration = _defaultAcceleration;

        _isDead = false;
        _kamikazeDamage = _defaultKamikazeDamage;
        _maxHealth = _defaultHealth;
        _currentHealth = _defaultHealth;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(_isDead)
            return;

        Entity entity = other.gameObject.GetComponentInChildren<Entity>();

        // Debug.Log("collision");

        if(entity != null)
        {
            entity.TakeDamage(this._kamikazeDamage);
            this.Die();
        }    
    }
}
