using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPool<T> : Singleton<EntityPool<T>> where T: iPoolableEntity<T>, Entity
{
    
}