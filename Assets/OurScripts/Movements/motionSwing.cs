using UnityEngine;

public class motionSwing : MonoBehaviour
{
    public bool enableSwing = true;
    public float length = 2f; // Length of the swing
    public float gravity = 9.81f; // Gravity effect
    public float maxPushForce = 1f; // Max force applied when pushing
    private float angle = 0f; // Swing angle in degrees
    private float angularVelocity = 0f; // How fast the swing is moving
    private bool isMoving = false;
    void Update()
    {
        if (!enableSwing) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            angularVelocity = 0f;
            return;
            }
        if (!isMoving) ApplyPush();
        float deltaTime = Time.deltaTime;

        // Calculate angular acceleration using pendulum physics: α = - (g / L) * sin(θ)
        float angularAcceleration = - (gravity / length) * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Update velocity with acceleration
        angularVelocity += angularAcceleration * deltaTime;

        // Apply friction (slows down movement naturally)

        // Update the swing angle based on velocity
        angle += angularVelocity * Mathf.Rad2Deg * deltaTime;

        // Apply rotation to the swing
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void ApplyPush()
    {
        // Determine push strength based on motion direction and angle
        float pushMultiplier = Mathf.Cos(angle * Mathf.Deg2Rad); // -1 when upside down, 1 at bottom
        float pushDirection = Mathf.Sign(angularVelocity); // 1 if moving forward, -1 if moving backward
        float pushForce = pushMultiplier * pushDirection * maxPushForce;

        // Apply push force to velocity
        angularVelocity += pushForce;
        isMoving = true;
    }
}