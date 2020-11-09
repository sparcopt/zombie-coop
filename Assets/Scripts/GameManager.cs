using System;
using System.Collections;
using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public GameState GameState = GameState.Ready;
    public ViewBase StartView;
    public GameObject PlayerPrefab;
    public Transform PlayerSpawnPoint;
    public EnemySpawner EnemySpawner;
    public Transform[] EnemySpawnPoints;
    public float SpawnDuration = 5f;
    public int MaxZombies = 2;
    private int zombiesSpawned = 0;
    private IEnumerator coSpawnEnemies;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        Instantiate(PlayerPrefab, PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);
        
        coSpawnEnemies = CoSpawnEnemies();
        
        StartCoroutine(coSpawnEnemies);

        GameState = GameState.Playing;
    }

    public void GameOver()
    {
        StopCoroutine(coSpawnEnemies);
        GameState = GameState.GameOver;
    }

    public void ResetGame()
    {
        // Clean up
        var enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        zombiesSpawned = 0;
        
        // Display Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        GameState = GameState.Ready;
        StartView.Show();
    }
    
    private IEnumerator CoSpawnEnemies()
    {
        yield return new WaitForSeconds(5);
        
        while (true)
        {
            foreach (var spawn in EnemySpawnPoints)
            {
                if (zombiesSpawned >= MaxZombies)
                {
                    print("skipping!");
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
