using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiTest : MonoBehaviour
{
    [SerializeField]
    private EnemyGemini _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy?.Initialize( new Queue<iEnemyAction>(
            new[] {
                new MoveTowards(new Vector2(0.75f, 3.24f)),
                new MoveTowards(new Vector2(0.75f, 3.5f)),
                new MoveTowards(new Vector2(-0.75f, 2.24f))
            }), 
            new Shoot(2), new WaitSeconds(20) , _enemy.Position
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
