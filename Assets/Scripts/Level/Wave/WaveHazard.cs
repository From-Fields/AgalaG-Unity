using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveHazard: iWaveUnit
{
    public Action<iWaveUnit> onUnitReleased { get; set; }

    private Sprite _sprite;
    private Hazard _hazard;
    private bool _rotate, _waitForTimeout;
    private float _speed, _rotationSpeed;
    private uint _damage, _health, _maxBounces;
    private Vector2 _position, _direction, _scale;

    public WaveHazard(
        Hazard hazard, Vector2 position, Vector2 direction, Sprite sprite = null,
        bool rotate = true, float speed = 5, float rotationSpeed = 1, 
        uint damage = 1, uint health = 1, uint maxBounces = 0, bool waitForTimeout = false,
        Vector2? scale = null, Action<iWaveUnit> onUnitReleased = null)
    {
        this.onUnitReleased = onUnitReleased;
        _hazard = hazard;
        _sprite = sprite;
        _rotate = rotate;
        _speed = speed;
        _rotationSpeed = rotationSpeed;
        _damage = damage;
        _health = health;
        _maxBounces = maxBounces;
        _position = position;
        _direction = direction;
        _scale = (scale.HasValue) ? scale.Value : new Vector2(0.6f, 0.6f);
        _waitForTimeout = waitForTimeout;
    }

    public void ExecuteTimeoutAction() { 
        if(_waitForTimeout)
            onUnitReleased?.Invoke(this);
    }

    public void Initialize() {
        _hazard.Initialize(
            _position, _direction, _sprite,_speed, _damage, _health, _rotate, _rotationSpeed, _scale, _maxBounces
        );

        if(!_waitForTimeout)
            CoroutineRunner.Instance.CallbackTimer(0.5f, () => onUnitReleased?.Invoke(this));
    } 

    public void Reserve() => _hazard.ReserveToPool();
}