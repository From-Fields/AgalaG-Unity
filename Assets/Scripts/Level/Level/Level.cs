using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Level/Level", fileName = "Level")]
public class Level: ScriptableObject, iLevel
{
    [SerializeField]
    private List<WaveController> _waveList = new List<WaveController>();
    [SerializeField]
    private Bounds _bounds;
    private Queue<WaveController> _waveQueue;

    public List<WaveController> WaveList => _waveList;
    public Bounds Bounds => _bounds;

    public bool HasNextWave {
        get {
            InitializeQueue();
            return (_waveQueue.Count > 0);
        }
    } 

    private void InitializeQueue() {
        if(_waveQueue == null)
            _waveQueue = new Queue<WaveController>(_waveList);
    }

    public WaveController GetNextWave() {
        InitializeQueue();

        if(_waveQueue.Count == 0)
            return null;

        return _waveQueue.Dequeue();
    }
}
