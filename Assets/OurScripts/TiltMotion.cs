using UnityEngine;

public class MotionSicknessTester : MonoBehaviour
{
    [Header("Tilt (fram/tilt bak)")]
    public bool enableTilt = false;
    public float tiltStrength = 10f;
    public float tiltSpeed = 1.5f;

    [Header("Wobble (sidorotation)")]
    public bool enableWobble = false;
    public float wobbleStrength = 5f;
    public float wobbleSpeed = 5f;

    [Header("Speed Variation (ryckig fart)")]
    public bool enableSpeedVariation = false;
    public float baseSpeed = 5f;
    public float speedVariation = 2f;

    [Header("Sudden Random Twist")]
    public bool enableRandomTwist = false;
    public float twistStrength = 10f;
    public float twistInterval = 5f;

    private float nextTwistTime = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        nextTwistTime = Time.time + twistInterval;
    }

    void Update()
    {
        Quaternion totalRotation = Quaternion.identity;

        // TILT (framåt/bakåt som gungbräda)
        if (enableTilt)
        {
            float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltStrength;
            totalRotation *= Quaternion.Euler(tilt, 0f, 0f);
        }

        // WOBBLE (vrider åt sidorna)
        if (enableWobble)
        {
            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleStrength;
            totalRotation *= Quaternion.Euler(0f, 0f, wobble);
        }

        // APPLY TOTAL ROTATION
        transform.localRotation = totalRotation;

        // SPEED VARIATION (ryckig fart framåt)
        if (enableSpeedVariation)
        {
            float variableSpeed = baseSpeed + Mathf.Sin(Time.time * 2f) * speedVariation;
            transform.position += transform.forward * variableSpeed * Time.deltaTime;
        }

        // SUDDEN TWIST
        if (enableRandomTwist && Time.time >= nextTwistTime)
        {
            float twist = Random.Range(-twistStrength, twistStrength);
            transform.Rotate(0f, twist, 0f);
            nextTwistTime = Time.time + twistInterval;
        }
    }
}
