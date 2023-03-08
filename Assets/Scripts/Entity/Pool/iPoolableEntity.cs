using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface iPoolableEntity<T> where T: MonoBehaviour, iPoolableEntity<T>, Entity
{
	public T OnCreate();
	public Action<T> onGetFromPool { get; }
	public Action<T> onReserve { get; }
    public IObjectPool<T> Pool { get; }
}