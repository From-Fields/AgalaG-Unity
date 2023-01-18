using System;
using System.Collections.Generic;
using UnityEngine;

public interface Entity
{
	public int health { get; }

    public void Move (Vector2 direction, float speed);
    public void Shoot();

    public void TakeDamage (int damage);
    public void Die();
}