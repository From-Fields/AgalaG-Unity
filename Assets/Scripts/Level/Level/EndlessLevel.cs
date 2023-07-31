using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevel: ScriptableObject, iLevel
{
    private List<WaveController> _waveList;
    private Unity.Mathematics.Random _seed;
    private Queue<WaveController> _waveQueue;
    private WaveController _previousWave, _currentWave;
    [SerializeField]
    private Bounds _bounds;

    public bool HasNextWave => true;
    
    public EndlessLevel(Queue<WaveController> waves, Bounds bounds) 
        :this(new List<WaveController>(waves), bounds) { }
    public EndlessLevel(List<WaveController> waves, Bounds bounds) {
        _waveList = waves;
        _seed = new Unity.Mathematics.Random();
        _bounds = bounds;
        ShuffleWaves();
    }

    private void ShuffleWaves() {
        IOrderedEnumerable<WaveController> randomizedList = _waveList.OrderBy(item => _seed.NextInt());
        _waveQueue = new Queue<WaveController>(randomizedList);
    }

    public List<WaveController> WaveList => new List<WaveController>(_waveList);
    public Bounds Bounds => _bounds;

    public WaveController GetNextWave() {
        _previousWave = _currentWave;

        if(_waveQueue.Count == 0) {
            do {
                ShuffleWaves();
                _currentWave = _waveQueue.Dequeue();
            } while(_currentWave == _previousWave);
        }
        else {
            _currentWave = _waveQueue.Dequeue();
        }

        return _currentWave;
    }
}