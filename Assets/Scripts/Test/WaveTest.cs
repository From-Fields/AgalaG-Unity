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
                    new Vector2(5, 6),
                    new MoveTowards(new Vector2(5, 3), 0.7f),
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
            }));

            wave.onWaveDone += CreateWave;
            wave.Initialize();
        }
}
