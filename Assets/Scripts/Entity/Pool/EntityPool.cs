using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EntityPool<T> : Singleton<EntityPool<T>> where T: MonoBehaviour, iPoolableEntity<T>, Entity
{
    private IObjectPool<T> _pool;
    private GameObject _prefab;
    private T _objReference;

    public IObjectPool<T> Pool {
        get {
            if(_pool == null)
                _pool = new ObjectPool<T>(ObjReference.OnCreate, ObjReference.OnGetFromPool, ObjReference.OnReserve);

            return _pool;
        }
    }

    public GameObject Prefab {
        get {
            if(_prefab == null)
                _prefab = PrefabRepository.Instance.GetPrefabOfType(typeof(T));
            
            return _prefab;
        }
    }

    public T ObjReference {
        get {
            if(_objReference == null)
                _objReference = Prefab.GetComponent<T>();

            return _objReference;
        }
    }
}