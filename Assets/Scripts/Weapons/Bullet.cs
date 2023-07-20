using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    private Sprite _sprite;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _direction = Vector3.zero;
    private float _speed = 0;
    private string _shooter;
    [SerializeField][Min(1)] private int _damage = 1;
    [SerializeField][Min(.01f)] private float _timeToDestroySelf = 1f;
    private float _counter = 0f;

    [SerializeField] private GameObject _explosion;
    private bool shouldExplode = false;

    void Awake() {
        _sprite = Resources.Load<Sprite>("Sprites/Bullet_Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision?.gameObject.GetComponent<Entity>();
        entity?.TakeDamage(_damage);
        DestroySelf();
    }

    void Update() {
        transform.position += _speed * Time.deltaTime * _direction;

        _counter += Time.deltaTime;
        if (_counter >= _timeToDestroySelf)
        {
            DestroySelf();
        }
    }

    /// <summary>
    /// Prepare Bullet to shoot and activate the gameobject
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    /// <param name="shooter"></param>
    /// <param name="sprite"></param>
    public void Initialize(Vector2 direction, float speed, string shooter, int damage = -1, Sprite sprite = null, 
        bool explosion = false) 
    {
        _direction = direction;
        _speed = speed;
        _shooter = shooter;
        _sprite = sprite ?? _sprite;
        if(_spriteRenderer != null && _spriteRenderer.sprite == null)
            _spriteRenderer.sprite = _sprite;
        _damage = (damage > 0) ? damage : _damage;
        shouldExplode = explosion;

        gameObject.SetActive(true);
    }

    private void CreateExplosion()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity).gameObject.layer = gameObject.layer;
    }

    private void DestroySelf()
    {
        if (shouldExplode)
        {
            CreateExplosion();
        }

        Destroy(gameObject);
    }

}
