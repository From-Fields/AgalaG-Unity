using System;
using System.Collections;
using UnityEngine;

public interface iEnemy: Entity
{
    public float DesiredSpeed { get; }
    public float CurrentAcceleration { get; }
    public Coroutine StartCoroutine(IEnumerator coroutine);
}