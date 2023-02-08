using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    Rigidbody2D _rigidbody;

    [SerializeField]
    GameObject target;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        this._defaultSpeed = 100;
        this._currentSpeed = _defaultSpeed;

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
        Debug.Log("OMAEWA MOU SHINDEIRU");
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
            Debug.Log("OWIE " + damage);
    }
}
