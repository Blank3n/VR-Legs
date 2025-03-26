using UnityEngine;

public class motionTilt : MonoBehaviour
{
    public bool enableTilt = true;
    public float tiltStrength = 10f;
    public float tiltSpeed = 1.5f;

    void Update()
    {
        if (!enableTilt) return;

        float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltStrength;
        transform.localRotation = Quaternion.Euler(tilt, 0f, 0f);
    }
}
