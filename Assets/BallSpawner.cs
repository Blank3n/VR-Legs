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
        Vector3 spawnPosition = new Vector3(Random.Range(-40f, 40f), spawnHeight, Random.Range(-40f, 40f));
        Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }
}
