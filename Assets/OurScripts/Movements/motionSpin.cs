using UnityEngine;

public class motionSpin : MonoBehaviour
{
    public bool enableSpin = false;
    public float rotSpeed = 1f; // Rotations per 5 seconds

    void Update()
    {
        if (!enableSpin)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            return;
        }

        float rotationAmount = (rotSpeed / 5f) * 360f * Time.time;

        transform.localRotation = Quaternion.Euler(0f, rotationAmount, 0f);
    }
}

