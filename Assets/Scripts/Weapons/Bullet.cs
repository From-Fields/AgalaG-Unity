using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Sprite _sprite;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Vector2 _direction = Vector2.zero;
    [SerializeField] private float _speed = 0;
    [SerializeField] private string _shooter;
    [SerializeField] private int _damage;

    void Awake() {
        _sprite = (Sprite) Resources.Load("Default_Sprite.png");
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(_shooter))
        {
            Entity entity = collision?.gameObject.GetComponent<Entity>();
            entity?.TakeDamage(_damage);
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
    public void Initialize(Vector2 direction, float speed, string shooter, Sprite sprite = null) {
        _direction = direction;
        _speed = speed;
        _shooter = shooter;
        _sprite = sprite ?? _sprite;

        gameObject.SetActive(true);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

}
