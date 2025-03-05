using UnityEngine;

public class SmoothFlyingPlatformWithProgressiveDimensionsAndTilting : MonoBehaviour
{
    public Vector3 circleCenter; // Center of the circular map
    public float initialCircleRadius = 10f; // Initial radius of the circular map
    public float maxCircleRadius = 20f; // Maximum radius the circle can expand to
    public float initialMinHeight = 0f; // Initial minimum height (Y position)
    public float initialMaxHeight = 10f; // Initial maximum height (Y position)
    public float maxHeightRange = 20f; // Maximum height range the platform can reach
    public float movementSpeed = 5f;
    public float minDistanceToTarget = 1f;
    public float maxDeviationAngle = 30f; // Maximum angle deviation from the current direction
    public float newTargetInterval = 5f;
    public float avoidanceDistance = 5f; // Distance to check for obstacles
    public LayerMask obstacleLayer; // LayerMask for obstacles

    public float progressionTime = 60f; // Time (in seconds) to fully progress to max dimensions
    public float tiltStartTime = 30f; // Time (in seconds) after which tilting starts
    public float maxTiltAngle = 15f; // Maximum tilt angle in degrees
    public float tiltSmoothness = 2f; // Smoothness of tilting (higher values = smoother)

    private Vector3 targetPosition;
    private float timer;
    private float progressionTimer;
    private float currentCircleRadius;
    private float currentMinHeight;
    private float currentMaxHeight;
    private Quaternion targetTilt;
    private bool isTilting = false;

    void Start()
    {
        currentCircleRadius = initialCircleRadius;
        currentMinHeight = initialMinHeight;
        currentMaxHeight = initialMaxHeight;
        SetRandomTargetPosition();
        targetTilt = Quaternion.identity; // Start with no tilt
    }

    void Update()
    {
        // Update progression timer
        progressionTimer += Time.deltaTime;

        // Progressively increase movement dimensions
        if (progressionTimer <= progressionTime)
        {
            float progress = progressionTimer / progressionTime;
            currentCircleRadius = Mathf.Lerp(initialCircleRadius, maxCircleRadius, progress);
            currentMinHeight = Mathf.Lerp(initialMinHeight, initialMinHeight - maxHeightRange / 2, progress);
            currentMaxHeight = Mathf.Lerp(initialMaxHeight, initialMaxHeight + maxHeightRange / 2, progress);
        }

        // Start tilting after tiltStartTime
        if (!isTilting && progressionTimer >= tiltStartTime)
        {
            isTilting = true;
        }

        // Move towards the target position
        if (Vector3.Distance(transform.position, targetPosition) <= minDistanceToTarget)
        {
            SetSmoothTargetPosition();
        }

        // Update the timer and set a new target position after the interval
        timer += Time.deltaTime;
        if (timer >= newTargetInterval)
        {
            SetSmoothTargetPosition();
            timer = 0f;
        }

        // Move the platform
        MovePlatform();

        // Apply smooth tilting
        if (isTilting)
        {
            SmoothTilt();
        }
    }

    void SetRandomTargetPosition()
    {
        // Generate a random position within the current circular map and height range
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomHeight = Random.Range(currentMinHeight, currentMaxHeight);
        targetPosition = circleCenter + new Vector3(randomDirection.x * currentCircleRadius, randomHeight, randomDirection.y * currentCircleRadius);
    }

    void SetSmoothTargetPosition()
    {
        // Calculate a new direction based on the current direction with a small deviation
        Vector3 currentDirection = (targetPosition - transform.position).normalized;
        Vector3 deviatedDirection = Quaternion.Euler(
            Random.Range(-maxDeviationAngle, maxDeviationAngle),
            Random.Range(-maxDeviationAngle, maxDeviationAngle),
            Random.Range(-maxDeviationAngle, maxDeviationAngle)
        ) * currentDirection;

        // Generate a new target position along the deviated direction
        Vector3 newTargetPosition = transform.position + deviatedDirection * Random.Range(5f, 15f); // Adjust range as needed

        // Clamp the new target position within the current circular map and height range
        Vector3 directionFromCenter = new Vector3(newTargetPosition.x - circleCenter.x, 0f, newTargetPosition.z - circleCenter.z);
        if (directionFromCenter.magnitude > currentCircleRadius)
        {
            directionFromCenter = directionFromCenter.normalized * currentCircleRadius;
            newTargetPosition.x = circleCenter.x + directionFromCenter.x;
            newTargetPosition.z = circleCenter.z + directionFromCenter.z;
        }
        newTargetPosition.y = Mathf.Clamp(newTargetPosition.y, currentMinHeight, currentMaxHeight);

        // Check if the new target position is valid (no obstacles in the way)
        if (!IsObstacleInPath(newTargetPosition))
        {
            targetPosition = newTargetPosition;
        }
    }

    void MovePlatform()
    {
        // Move towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;

        // Avoid obstacles
        AvoidObstacles();
    }

    void AvoidObstacles()
    {
        // Cast a ray in the direction of movement to detect obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out hit, avoidanceDistance, obstacleLayer))
        {
            // Calculate a new direction to avoid the obstacle
            Vector3 avoidanceDirection = Vector3.Reflect((targetPosition - transform.position).normalized, hit.normal);
            targetPosition = transform.position + avoidanceDirection * Random.Range(5f, 10f); // Adjust range as needed

            // Clamp the new target position within the current circular map and height range
            Vector3 directionFromCenter = new Vector3(targetPosition.x - circleCenter.x, 0f, targetPosition.z - circleCenter.z);
            if (directionFromCenter.magnitude > currentCircleRadius)
            {
                directionFromCenter = directionFromCenter.normalized * currentCircleRadius;
                targetPosition.x = circleCenter.x + directionFromCenter.x;
                targetPosition.z = circleCenter.z + directionFromCenter.z;
            }
            targetPosition.y = Mathf.Clamp(targetPosition.y, currentMinHeight, currentMaxHeight);
        }
    }

    bool IsObstacleInPath(Vector3 target)
    {
        // Check if there is an obstacle between the current position and the target position
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target, out hit, obstacleLayer))
        {
            return true; // Obstacle detected
        }
        return false; // No obstacle
    }

    void SmoothTilt()
    {
        // Smoothly interpolate towards the target tilt
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTilt, tiltSmoothness * Time.deltaTime);

        // If close to the target tilt, generate a new random tilt
        if (Quaternion.Angle(transform.rotation, targetTilt) < 1f)
        {
            targetTilt = Quaternion.Euler(
                Random.Range(-maxTiltAngle, maxTiltAngle),
                Random.Range(-maxTiltAngle, maxTiltAngle),
                Random.Range(-maxTiltAngle, maxTiltAngle)
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wireframe circle to visualize the current circular map in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(circleCenter, currentCircleRadius);

        // Draw lines to visualize the current height range
        Gizmos.color = Color.blue;
        Vector3 bottomCenter = new Vector3(circleCenter.x, currentMinHeight, circleCenter.z);
        Vector3 topCenter = new Vector3(circleCenter.x, currentMaxHeight, circleCenter.z);
        Gizmos.DrawLine(bottomCenter, topCenter);

        // Draw a line to the target position
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}