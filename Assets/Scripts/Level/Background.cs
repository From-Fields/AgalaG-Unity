using System;
using System.Collections.Generic;
using UnityEngine;

public class Background: MonoBehaviour
{
    private SpriteRenderer _sprite;

    [SerializeField]
    private Vector2 _speed;
    private Vector2 _currentOffset;

    public void SetSpeed(Vector2 newSpeed) => this._speed = newSpeed;

    private void Awake() {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.material = new Material(_sprite.material);
    }

    private void Update() {
        _sprite.material.mainTextureOffset += _speed * Time.deltaTime;
    }
}