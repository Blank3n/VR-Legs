using UnityEngine;

public class SkateboardSwing : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingAngle = 45f; // Maximum swing angle in degrees
    public float swingSpeed = 1f;  // Speed of the swing
    public float damping = 0.98f;  // Damping factor to gradually reduce swinging
    public int fullRotations = 1;  // Number of full rotations to complete
    
    private bool isSwinging = false;
    private float currentAngle = 0f;
    private float angularVelocity = 0f;
    private int swingCount = 0;
    private float totalRotation = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (isSwinging)
        {
            // Calculate swing motion using angular velocity and damping
            angularVelocity += -swingSpeed * Mathf.Sin(currentAngle * Mathf.Deg2Rad) * Time.deltaTime;
            angularVelocity *= damping;
            currentAngle += angularVelocity * Mathf.Rad2Deg * Time.deltaTime;
            
            // Apply rotation to the skateboard
            transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, currentAngle);
            
            // Track total rotation
            totalRotation += Mathf.Abs(angularVelocity * Mathf.Rad2Deg * Time.deltaTime);
            
            // Check if we've completed the required rotations
            if (totalRotation >= 360f * fullRotations)
            {
                isSwinging = false;
                // Snap back to initial rotation
                transform.rotation = initialRotation;
            }
        }
    }

    // Call this method when the last waypoint is reached
    public void StartSwinging()
    {
        if (!isSwinging)
        {
            isSwinging = true;
            currentAngle = 0f;
            angularVelocity = swingSpeed * Mathf.Deg2Rad * swingAngle * 0.5f; // Initial push
            totalRotation = 0f;
            swingCount = 0;
        }
    }

    // Call this if you need to stop swinging prematurely
    public void StopSwinging()
    {
        isSwinging = false;
        transform.rotation = initialRotation;
    }
}