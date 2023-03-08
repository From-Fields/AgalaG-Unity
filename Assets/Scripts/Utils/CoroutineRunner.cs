using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : SingletonMonoBehaviour<CoroutineRunner>
{
    public Coroutine CallbackTimer(float timeout, Action callback = null)
    {
        return StartCoroutine(SetTimer(timeout, callback));
    }

    private IEnumerator SetTimer(float timeout, Action callback = null)
    {
        yield return new WaitForSeconds(timeout);

        callback?.Invoke();
    }
}