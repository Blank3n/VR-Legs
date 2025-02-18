using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnInterval = 2f;
    public float spawnHeight = 200f;

    void Start()
    {
        InvokeRepeating("SpawnBall", 5f, spawnInterval);
    }

    void SpawnBall()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-4f, 4f), spawnHeight, Random.Range(-4f, 4f));
        Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }
}
