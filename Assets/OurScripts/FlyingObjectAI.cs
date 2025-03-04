using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FlyingPlatformController : MonoBehaviour
{
    public Vector3 mapDimensions = new Vector3(50, 10, 50); // Define map size in the Inspector
    public float movementSpeed = 5f;
    public float minDistanceToTarget = 1f;
    public float newTargetInterval = 5f;

    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    private float timer;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Move towards the target position
        if (Vector3.Distance(transform.position, targetPosition) <= minDistanceToTarget)
        {
            SetRandomTargetPosition();
        }

        // Update the timer and set a new target position after the interval
        timer += Time.deltaTime;
        if (timer >= newTargetInterval)
        {
            SetRandomTargetPosition();
            timer = 0f;
        }
    }

    void SetRandomTargetPosition()
    {
        // Generate a random position within the map dimensions
        Vector3 randomPosition = new Vector3(
            Random.Range(-mapDimensions.x / 2, mapDimensions.x / 2),
            Random.Range(-mapDimensions.y / 2, mapDimensions.y / 2),
            Random.Range(-mapDimensions.z / 2, mapDimensions.z / 2)
        );

        // Use NavMesh to find a valid position
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, mapDimensions.magnitude, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wireframe cube to visualize the map dimensions in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, mapDimensions);
    }
}