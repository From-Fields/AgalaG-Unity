using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoroutineRunner : SingletonMonoBehaviour<CoroutineRunner>
{
    private List<Coroutine> _routines = new();
    private Action<Coroutine> routineCompleted;

    public Coroutine CallbackTimer(float timeout, Action callback = null)
    {
        RoutineContainer container = new();
        container.routine = StartCoroutine(SetTimer(timeout, container, callback));
        _routines.Add(container.routine);
        return container.routine;
    }

    public void CancelCallback(Coroutine coroutine)
    {
        if(coroutine == null)
            return;

        StopCoroutine(coroutine);
        RemoveRoutine(coroutine);
    }

    private IEnumerator SetTimer(float timeout, RoutineContainer container, Action callback = null)
    {
        yield return new WaitForSeconds(timeout);

        callback?.Invoke();
        routineCompleted?.Invoke(container.routine);
    }

    private void RemoveRoutine(Coroutine routine) {
        _routines.Remove(routine);
    }

    protected override void SingletonAwakened() {
        routineCompleted += RemoveRoutine;
    }

    public void InterruptAllRoutines() {
        foreach (var item in _routines) {
            StopCoroutine(item);
        }

        _routines = new List<Coroutine>();
    }

    private class RoutineContainer {
        public Coroutine routine;
    }
}