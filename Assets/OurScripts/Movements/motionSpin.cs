using UnityEngine;

public class motionSpin : MonoBehaviour
{
    public bool enableSpin = false;
    public float rotSpeed = 1f; // Number of rotations per 5 seconds

    void Update()
    {
        if (!enableSpin) return;

        // Calculate the rotation based on the time and speed
        float rotationAmount = (rotSpeed / 5f) * 360f * Time.time; // Rotations per second, scaled by 360 degrees

        // Apply the rotation around the Y-axis
        transform.rotation = Quaternion.Euler(0f, rotationAmount, 0f);
    }
}
