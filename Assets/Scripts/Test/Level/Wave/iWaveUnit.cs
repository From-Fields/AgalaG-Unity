using System;
using System.Collections.Generic;
using UnityEngine;

public interface iWaveUnit
{
    public Action<iWaveUnit> onUnitReleased { get; set; }
	public void Initialize(Bounds levelBounds);
    public void ExecuteTimeoutAction();
    public void Reserve();
}