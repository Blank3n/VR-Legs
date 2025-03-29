using UnityEngine;

public class SkateboardRotation : MonoBehaviour
{
    [Header("Banking Settings")]
    public float maxBankAngle = 30f; // Max tilt angle in degrees
    public float bankSpeed = 5f;     // How quickly it tilts
    public float lookAheadDistance = 2f; // How far ahead to check waypoints

    private ModularPathMovement pathMovement;
    private Rigidbody rb;
    private Transform skateboardVisual; // The actual skateboard model (for tilting)

    void Start()
    {
        pathMovement = GetComponentInParent<ModularPathMovement>();
        rb = GetComponent<Rigidbody>();
        skateboardVisual = transform.GetChild(0); // Assuming skateboard is first child
    }

    void FixedUpdate()
    {
        if (pathMovement == null || pathMovement.allWaypoints.Count == 0) return;

        // Get current and next waypoint
        int currentIndex = pathMovement.currentIndex;
        Transform currentWaypoint = pathMovement.allWaypoints[currentIndex];
        Transform nextWaypoint = pathMovement.allWaypoints[(currentIndex + 1) % pathMovement.allWaypoints.Count];

        // Calculate direction change
        Vector3 currentDir = (currentWaypoint.position - transform.position).normalized;
        Vector3 nextDir = (nextWaypoint.position - currentWaypoint.position).normalized;

        // Calculate banking angle (how sharp the turn is)
        float turnAngle = Vector3.SignedAngle(currentDir, nextDir, Vector3.up);
        float targetBankAngle = -Mathf.Clamp(turnAngle, -maxBankAngle, maxBankAngle);

        // Smoothly tilt the skateboard
        Quaternion targetTilt = Quaternion.Euler(0, 0, targetBankAngle);
        skateboardVisual.localRotation = Quaternion.Lerp(
            skateboardVisual.localRotation,
            targetTilt,
            Time.fixedDeltaTime * bankSpeed
        );

        // Still face forward (optional: adjust if needed)
        if (rb.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * pathMovement.rotationSpeed
            );
        }
    }
}