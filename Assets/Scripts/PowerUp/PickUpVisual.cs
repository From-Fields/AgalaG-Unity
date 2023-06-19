using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PickUpVisual : MonoBehaviour
{
    // Options
    [SerializeField]
    private bool _debug = false;
    [SerializeField]
    private bool _rotate, _doScale;
    [SerializeField]
    private float _rotationSpeed, _maximumScale, _scaleSpeed;

    // References
    private SpriteRenderer _renderer;

    // Local Variables
    private bool _scaleUp = true;

    internal void Initialize(Sprite sprite, bool rotate, float rotationSpeed, bool doScale, float maximumScale, float scaleSpeed) {
        gameObject.SetActive(true);

        this._renderer.sprite = sprite;

        this._rotate = rotate;
        this._doScale = doScale;
        this._rotationSpeed = rotationSpeed;
        this._maximumScale = maximumScale;
        this._scaleSpeed = scaleSpeed;
    }

    private void DoScale() {
        if(!_doScale)
            return;

        Vector2 targetScale = (_scaleUp) ? Vector2.one * _maximumScale : Vector2.one;

        transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * _scaleSpeed);

        if(Vector2.Distance(transform.localScale, targetScale) <= 0.05f)
            _scaleUp = !_scaleUp;
    }
    private void DoRotation() {
        if(!_rotate)
            return;

        transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));
    }

    // Unity Hooks
    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();

        #if UNITY_EDITOR
        if(!_debug)
        #endif
            gameObject.SetActive(false);
    }

    private void Update() {
        DoRotation();
        DoScale();
    }
}
