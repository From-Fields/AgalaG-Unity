using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController
{
    private bool _isDone;
    private float _timeout;
    public Action onWaveDone;
    private List<iWaveUnit> _unitList, _unitCache;

    public bool IsDone => _isDone;

    public WaveController(float timeout, List<iWaveUnit> unitList)
    {
        this._isDone = false;
        this._timeout = timeout;
        this._unitCache = unitList;
    }
    public void Initialize(Bounds levelBounds)
    {
        this._isDone = false;
        _unitList = new List<iWaveUnit>(_unitCache);

        foreach (iWaveUnit unit in _unitList)
        {
            unit.onUnitReleased += RemoveUnitFromWave;
            unit.Initialize(levelBounds);
        }

        CoroutineRunner.Instance.CallbackTimer(_timeout, TimeOutAllUnits);
    }
    private void RemoveUnitFromWave(iWaveUnit unit)
    {
        unit.onUnitReleased -= RemoveUnitFromWave;
        _unitList.Remove(unit);

        if(_unitList.Count == 0)
        {
            onWaveDone?.Invoke();
            _isDone = true;
        }
    }
    private void TimeOutAllUnits()
    {
        int unitCount = _unitList.Count;
        for (int i = 0; i < unitCount; i++) {
            iWaveUnit unit = _unitList[i];
            unit.ExecuteTimeoutAction();
        }

        CoroutineRunner.Instance.CallbackTimer(_timeout, EliminateAllUnits);
    }
    private void EliminateAllUnits()
    {
        int unitCount = _unitList.Count;

        for (int i = 0; i < unitCount; i++) {
            if(_isDone)
                return;

            _unitList[i]?.Reserve();
        }
        if(_isDone)
            return;

        onWaveDone?.Invoke();
    }
}
