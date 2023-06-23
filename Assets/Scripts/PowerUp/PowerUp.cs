using System;
using System.Collections.Generic;
using UnityEngine;

public interface iPowerUp
{
    public bool IsInstant { get; }
    
    public void OnPickup(Player player);
    public void OnTick();
    public int OnTakeDamage(int damage, int playerHealth);
    public bool OnDeath();
    public void OnEnd();
}

[Serializable]
public abstract class PowerUp: iPowerUp 
{
    protected Player _player;
    
    public abstract Sprite Sprite { get; }
    public abstract bool IsInstant { get; }

    protected void EndPowerUp() => OnEnd();

    public virtual void OnTick() {}
    public virtual int OnTakeDamage(int damage, int playerHealth) => damage;
    public virtual bool OnDeath() => true;

    public abstract void OnPickup(Player player);
    public abstract void OnEnd();
}