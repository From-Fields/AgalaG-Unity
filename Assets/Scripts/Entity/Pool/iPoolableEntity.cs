using System;
using System.Collections.Generic;

public interface iPoolableEntity<T> where T: iPoolableEntity<T>, Entity
{
	public Action<T> OnReserve { get; set; }
    public EntityPool<T> Pool { get; }
}