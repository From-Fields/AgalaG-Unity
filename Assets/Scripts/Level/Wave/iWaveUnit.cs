using System;
using System.Collections.Generic;

public interface iWaveUnit
{
    public Action<iWaveUnit> onUnitReleased { get; set; }
	public void Initialize();
    public void ExecuteTimeoutAction();
    public void Reserve();
}