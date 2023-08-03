using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CreateWave()
    {        
        WaveController wave = new WaveController(10, new List<iWaveUnit>(new iWaveUnit[] {
            new WaveUnit<EnemyBumblebee>(
                new Vector2(0, 9),
                new MoveTowards(new Vector2(0, 3), 0.7f),
                new MoveTowards(new Vector2(6, -6), 1.5f),
                new Queue<iEnemyAction>(new [] {
                    new MoveAndShoot(new Vector2(2, 0), 1.5f, 1, 0.8f)
                }),
                drop: new ShieldPowerUp()
            ),
            new WaveUnit<EnemyGemini>(
                new Vector2(-5, 6),
                new MoveTowards(new Vector2(-5, 3)),
                new MoveTowards(new Vector2(-6, -6)),
                new Queue<iEnemyAction>(new[] {
                    new MoveAndShoot(new Vector2(-4, -3))
                }),
                drop: new RepairPowerUp()
            ),
            new WaveHazard(
                new Vector2(7.4f, 5.7f), 
                new Vector2(-0.2f, -1),
                maxBounces: 3,
                waitForTimeout: true
            ),
        }));

        wave.onWaveDone += CreateWave;
        wave.Initialize(new Bounds(new Vector3(0, 0), new Vector3(18.8f, 11.1f)));
    }

    

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(0, 0), new Vector3(18.8f, 11.1f));
    }
}
