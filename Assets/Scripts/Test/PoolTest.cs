using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolTest : MonoBehaviour
{
    private Queue<Vector2> _positions;

    [SerializeField]
    private int _spawnInterval;
    [SerializeField]
    private List<Vector2> positions;

    private IObjectPool<EnemyKamikaze> _pool;

    // Start is called before the first frame update
    void Awake()
    {
        this._pool = EntityPool<EnemyKamikaze>.Instance.Pool;
        this._positions = new Queue<Vector2>(positions);
        StartCoroutine(WaitAndPull(_spawnInterval));
    }

    private IEnumerator WaitAndPull(int interval)
    {
        yield return new WaitForSeconds(interval);

        PullFromPool();
    }

    private void PullFromPool()
    {
        EnemyKamikaze enemy = _pool.Get();
        Vector2 position = _positions.Dequeue();
        iEnemyAction action = new WaitSeconds(1);
        Queue<iEnemyAction> actions = new Queue<iEnemyAction>(new[] {action});
        enemy.Initialize(actions, action, action, position);

        _positions.Enqueue(position);

        Debug.Log(_pool.CountInactive);
        
        StartCoroutine(WaitAndPull(_spawnInterval));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
