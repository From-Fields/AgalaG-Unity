using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumblebeeTest : MonoBehaviour
{
    [SerializeField]
    private EnemyBumblebee _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy?.Initialize( new Queue<iEnemyAction>(
            new[] {
                new MoveAndShoot(new Vector2(2.75f, 3.25f), speedModifier: 1, trackingSpeed: 1, stopOnEnd: false, decelerate: false),
                new MoveAndShoot(new Vector2(0.75f, 1.25f), speedModifier: 1, trackingSpeed: 0.1f, stopOnEnd: false),
                new MoveAndShoot(new Vector2(-1.75f, -1.25f), speedModifier: 1, trackingSpeed: 1)
            }), 
            new Shoot(2), new WaitSeconds(20) , _enemy.Position,
            new Bounds(new Vector3(0, 0), new Vector3(11.1f, 18.8f))
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
