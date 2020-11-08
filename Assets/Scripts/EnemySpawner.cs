using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ZombiePrefab;
    public GameObject SpawnAt(Vector3 position, Quaternion rotation) =>
        Instantiate(ZombiePrefab, position, rotation);
}
