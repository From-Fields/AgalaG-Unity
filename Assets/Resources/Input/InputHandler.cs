using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler: SingletonMonoBehaviour<InputHandler>
{
    public BasicInput inputScheme { get; private set; }

    public bool HasMovement => GetMovement().inProgress;
    public Vector2 Movement => GetMovement().ReadValue<Vector2>();
    private InputAction GetMovement() {
        return inputScheme.PlayerActions.Movement;
    }

    public bool Pause => GetPause().triggered;
    private InputAction GetPause() {
        return inputScheme.PlayerActions.Pause;
    }

    public bool Shoot => GetShoot().inProgress;
    private InputAction GetShoot() {
        return inputScheme.PlayerActions.Shoot;
    }

    // Unity Hooks
    override
    protected void SingletonAwakened() {
        base.SingletonAwakened();
        this.inputScheme = new BasicInput();
    }
    private void OnEnable() {
        inputScheme?.Enable();
    }
    private void OnDisable() {
        inputScheme?.Disable();
    }
}