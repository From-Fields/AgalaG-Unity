using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface iPoolableObject<T> where T: MonoBehaviour, iPoolableObject<T> {
	public T OnCreate();
	public Action<T> onGetFromPool { get; }
	public Action<T> onReleaseToPool { get; }
    public IObjectPool<T> Pool { get; }
}

public interface iPoolableEntity<T>: iPoolableObject<T> where T: MonoBehaviour, iPoolableEntity<T>, Entity
{}