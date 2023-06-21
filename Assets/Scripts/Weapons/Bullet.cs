using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake() {
        _sprite = Resources.Load<Sprite>("Sprites/Bullet_Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(_shooter))
        {
            Entity entity = collision?.gameObject.GetComponent<Entity>();
            entity?.TakeDamage(_damage);
            DestroySelf();
        }
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
    public void Initialize(Vector2 direction, float speed, string shooter, int damage = -1, Sprite sprite = null) {
        _direction = direction;
        _speed = speed;
        _shooter = shooter;
        _sprite = sprite ?? _sprite;
        if(_spriteRenderer != null && _spriteRenderer.sprite == null)
            _spriteRenderer.sprite = _sprite;
        _damage = (damage > 0) ? damage : _damage;

        gameObject.SetActive(true);
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }

}
