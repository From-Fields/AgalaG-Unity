using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolTest : MonoBehaviour
{
    private int _score = 0;
    private Queue<Vector2> _positions;

    [SerializeField]
    private float _spawnInterval;
    [SerializeField]
    private List<Vector2> positions;

    // Start is called before the first frame update
    void Awake()
    {
        this._positions = new Queue<Vector2>(positions);
        StartCoroutine(WaitAndPull(_spawnInterval));
    }

    private IEnumerator WaitAndPull(float interval)
    {
        yield return new WaitForSeconds(interval);

        PullFromPool();
    }

    private void PullFromPool()
    {
        Vector2 position = _positions.Dequeue();
        iEnemyAction action = new WaitSeconds(1);
        Queue<iEnemyAction> actions = new Queue<iEnemyAction>(new[] {action});
        iWaveUnit unit = new WaveUnit<EnemyKamikaze>(position, action, action, actions, Score);
        unit.Initialize(new Bounds(new Vector3(0, 0), new Vector3(11.1f, 18.8f)));

        _positions.Enqueue(position);
        
        StartCoroutine(WaitAndPull(_spawnInterval));
    }

    private void Score(int score)
    {
        _score += score;
        Debug.Log(_score);
    }
}
