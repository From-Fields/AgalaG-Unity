using System;
using System.Collections.Generic;
using UnityEngine;

public interface iLevel {
    public bool HasNextWave { get; }
    public Bounds Bounds { get; }
    public List<WaveController> WaveList { get; }
    public WaveController GetNextWave();
}