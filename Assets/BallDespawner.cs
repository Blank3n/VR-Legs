using UnityEngine;

public class BallDespawner : MonoBehaviour
{
    public float despawnHeight = -10f; // Change this threshold as needed

    void Update()
    {
        if (transform.position.y < despawnHeight)
        {
            Destroy(gameObject); // Destroy the ball when it falls too low
        }
    }
}
