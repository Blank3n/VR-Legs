using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class SwingMotion : MonoBehaviour
{
    public float length = 2f; // Length of the swing
    public float gravity = 9.81f; // Gravity effect
    public float maxPushForce = 1f; // Max force applied when pushing
    public float friction = 0.999f; // Friction factor to slow the swing over time

    public InputAction pushAction; // Input action for pushing

    private float angle = 0f; // Swing angle in degrees
    private float angularVelocity = 0f; // How fast the swing is moving
    private bool isPushing = false; // Prevents holding push indefinitely

    void OnEnable() => pushAction.Enable();
    void OnDisable() => pushAction.Disable();

    void Update()
    {
        float deltaTime = Time.deltaTime;

        // Calculate angular acceleration using pendulum physics: α = - (g / L) * sin(θ)
        float angularAcceleration = - (gravity / length) * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Update velocity with acceleration
        angularVelocity += angularAcceleration * deltaTime;

        // Apply friction (slows down movement naturally)
        angularVelocity *= friction;

        // Update the swing angle based on velocity
        angle += angularVelocity * Mathf.Rad2Deg * deltaTime;

        // Apply rotation to the swing
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        // Check for player push input
        if ((pushAction.WasPressedThisFrame() || IsVRButtonPressed() || IsXRSimulatorPressed()) && !isPushing)
        {
            ApplyPush();
        }
    }

    void ApplyPush()
    {
        // Determine push strength based on motion direction and angle
        float pushMultiplier = Mathf.Cos(angle * Mathf.Deg2Rad); // -1 when upside down, 1 at bottom
        float pushDirection = Mathf.Sign(angularVelocity); // 1 if moving forward, -1 if moving backward
        float pushForce = pushMultiplier * pushDirection * maxPushForce;

        // Apply push force to velocity
        angularVelocity += pushForce;
        isPushing = true;

        // Reset push state after a short delay
        Invoke(nameof(ResetPush), 0.3f);
    }

    void ResetPush() => isPushing = false;

    bool IsVRButtonPressed()
    {
        UnityEngine.XR.InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        return device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool pressed) && pressed;
    }

    bool IsXRSimulatorPressed() => Mouse.current.leftButton.wasPressedThisFrame;
}
