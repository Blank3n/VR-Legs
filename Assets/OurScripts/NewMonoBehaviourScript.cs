using UnityEngine;
using System.Collections.Generic;

public class FlyingPlatformPathGenerator : MonoBehaviour
{
    [Header("Path Settings")]
    public int pathPoints = 10; // Number of waypoints in the path
    public Vector3 minBounds = new Vector3(100, 10, -75);
    public Vector3 maxBounds = new Vector3(175, 50, -125);
    public float minDistanceBetweenPoints = 10f;
    
    [Header("Movement Settings")]
    public float startSpeed = 5f;
    public float maxSpeed = 50f;
    public float acceleration = 0.5f;
    public float rotationSpeed = 2f;
    public float tiltAmount = 15f; // Maximum random tilt angle

    private List<Vector3> path;
    private int currentTargetIndex = 0;
    private float currentSpeed;
    
    void Start()
    {
        currentSpeed = startSpeed;
        GeneratePath();
        StartCoroutine(FollowPath());
    }

    void GeneratePath()
    {
        path = new List<Vector3>();
        Vector3 lastPoint = transform.position;

        for (int i = 0; i < pathPoints; i++)
        {
            Vector3 randomPoint;
            int attempts = 0;
            do
            {
                randomPoint = new Vector3(
                    Random.Range(minBounds.x, maxBounds.x),
                    Random.Range(minBounds.y, maxBounds.y),
                    Random.Range(minBounds.z, maxBounds.z)
                );
                attempts++;
            }
            while (Physics.Raycast(lastPoint, (randomPoint - lastPoint).normalized, Vector3.Distance(lastPoint, randomPoint)) && attempts < 10);

            if (attempts < 10)
            {
                path.Add(randomPoint);
                lastPoint = randomPoint;
            }
        }
    }

    IEnumerator FollowPath()
    {
        while (currentTargetIndex < path.Count)
        {
            Vector3 targetPosition = path[currentTargetIndex];
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.5f)
            {
                // Move toward the target position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

                // Rotate smoothly toward the next waypoint
                Vector3 direction = (targetPosition - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Apply random tilting effect
                transform.rotation *= Quaternion.Euler(Random.Range(-tiltAmount, tiltAmount), 0, Random.Range(-tiltAmount, tiltAmount));

                // Increase speed over time
                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

                yield return null;
            }

            currentTargetIndex++;
        }
    }
}
