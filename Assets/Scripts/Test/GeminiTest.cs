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
                new MoveAndShoot(new Vector2(2.75f, 3.25f), speedModifier: 1, trackingSpeed: 10, stopOnEnd: false),
                new MoveAndShoot(new Vector2(0.75f, 1.25f), speedModifier: 1, trackingSpeed: 10, stopOnEnd: false),
                new MoveAndShoot(new Vector2(-1.75f, -1.25f), speedModifier: 1, trackingSpeed: 10)
            }), 
            new Shoot(2), new WaitSeconds(20) , _enemy.Position
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
