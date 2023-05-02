using System;
using System.Collections.Generic;

public interface iLevel {
    public bool HasNextWave { get; }
    public List<WaveController> WaveList { get; }
    public WaveController GetNextWave();
}