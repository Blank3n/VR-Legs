using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class SwingMotion : MonoBehaviour
{
    public float length = 2f; // Length of the swing (affects period)
    public float gravity = 9.81f; // Acceleration due to gravity
    public float maxPushForce = 1f; // How much velocity each input adds
    public InputAction pushAction; // New Input System action for pushing

    private float angle = 0f; // Swing angle in degrees
    private float angularVelocity = 0f; // Current speed of swing
    private bool isPushing = false; // To prevent holding push indefinitely

    void OnEnable() => pushAction.Enable();
    void OnDisable() => pushAction.Disable();

    void Update()
    {
        float deltaTime = Time.deltaTime;

        // Calculate angular acceleration using the physics formula: α = - (g / L) * sin(θ)
        float angularAcceleration = - (gravity / length) * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Update velocity and angle using Euler integration
        angularVelocity += angularAcceleration * deltaTime;
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
        // The push should add speed in the direction the swing is moving
        float pushDirection = Mathf.Sign(angularVelocity);
        angularVelocity += pushDirection * maxPushForce;
        isPushing = true;

        // Reset push state after a short delay (prevents constant push spam)
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
