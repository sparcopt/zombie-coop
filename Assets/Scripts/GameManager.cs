using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EnemySpawner EnemySpawner;
    public Transform[] EnemySpawnPoints;
    public float SpawnDuration = 5f;
    public int MaxZombies = 2;
    private int zombiesSpawned = 0;
    
    private void Start()
    {
        StartCoroutine(CoSpawnEnemies());
    }
    
    private IEnumerator CoSpawnEnemies()
    {
        while (true)
        {
            foreach (var spawn in EnemySpawnPoints)
            {
                if (zombiesSpawned >= MaxZombies)
                {
                    continue;
                }

                var enemyObj = EnemySpawner.SpawnAt(spawn.position, spawn.rotation);
                var enemy = enemyObj.GetComponent<Enemy>();
                
                enemy.OnDead.AddListener(() =>
                {
                    print("Zombie is DEAD");
                    zombiesSpawned--;
                });

                zombiesSpawned++;
            }
            
            yield return new WaitForSeconds(SpawnDuration);
        }
    }
}
