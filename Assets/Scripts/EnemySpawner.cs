using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ZombiePrefab;
    [HideInInspector]
    public Transform[] EnemySpawnPoints;
    public float SpawnDuration = 5f;

    void Start()
    {
        EnemySpawnPoints = new Transform[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            EnemySpawnPoints[i] = transform.GetChild(i);
        }

        StartCoroutine(CoStartSpawning());
    }

    private IEnumerator CoStartSpawning()
    {
        while(true)
        {
            for(int i = 0; i < EnemySpawnPoints.Length; i++)
            {
                Instantiate(ZombiePrefab, EnemySpawnPoints[i].position, EnemySpawnPoints[i].rotation);
            }

            yield return new WaitForSeconds(SpawnDuration);
        }
    }
}
