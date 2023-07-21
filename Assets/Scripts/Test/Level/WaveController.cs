using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController
{
    private bool _isDone;
    private float _timeout;
    public Action onWaveDone;
    private List<iWaveUnit> unitList;

    public bool IsDone => _isDone;

    public WaveController(float timeout, List<iWaveUnit> unitList)
    {
        this._isDone = false;
        this._timeout = timeout;
        this.unitList = unitList;
    }
    public void Initialize(Bounds levelBounds)
    {
        foreach (iWaveUnit unit in unitList)
        {
            unit.onUnitReleased += RemoveUnitFromWave;
            unit.Initialize(levelBounds);
        }

        CoroutineRunner.Instance.CallbackTimer(_timeout, TimeOutAllUnits);
    }
    private void RemoveUnitFromWave(iWaveUnit unit)
    {
        unitList.Remove(unit);

        if(unitList.Count == 0)
        {
            onWaveDone?.Invoke();
            _isDone = true;
        }
    }
    private void TimeOutAllUnits()
    {
        int unitCount = unitList.Count;
        for (int i = 0; i < unitCount; i++) {
            iWaveUnit unit = unitList[i];
            unit.ExecuteTimeoutAction();
        }

        CoroutineRunner.Instance.CallbackTimer(_timeout, EliminateAllUnits);
    }
    private void EliminateAllUnits()
    {
        int unitCount = unitList.Count;

        for (int i = 0; i < unitCount; i++) {
            if(_isDone)
                return;

            unitList[i]?.Reserve();
        }
        if(_isDone)
            return;

        onWaveDone?.Invoke();
    }
}
