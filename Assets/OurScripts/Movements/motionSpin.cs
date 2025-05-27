using UnityEngine;

public class motionSpin : MonoBehaviour
{
    public bool enableSpin = false;
    public float rotSpeed = 1f; // Rotations per 5 seconds

    private bool enabled = false;
    private float compAmount = 0;
    void Update()
    {
        if (!enableSpin)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            enabled = false;
            return;
        }

        float rotationAmount = (rotSpeed / 5f) * 360f * Time.time;

        if (!enabled)
        {
            enabled = true;
            compAmount = -rotationAmount;
        }

        float finalRotation = rotationAmount + compAmount;
        transform.localRotation = Quaternion.Euler(0f, finalRotation, 0f);
    }
}

