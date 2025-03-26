using UnityEngine;

public class motionTester : MonoBehaviour
{
    [Header("Tilt (fram/bak)")]
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

    public enum TwistFrequency
    {
        Slow,
        Normal,
        Fast
    }

    [Header("Sudden Random Twist")]
    public bool enableRandomTwist = false;
    public TwistFrequency twistFrequency = TwistFrequency.Normal;
    public float twistStrength = 10f;
    public float twistDuration = 1f;

    private float nextTwistTime = 0f;
    private Quaternion targetRotation;
    private bool isTwisting = false;
    private float twistTimer = 0f;

    void Start()
    {
        nextTwistTime = Time.time + GetTwistInterval();
        targetRotation = transform.rotation;
    }

    void Update()
    {
        Quaternion totalRotation = Quaternion.identity;

        // TILT
        if (enableTilt)
        {
            float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltStrength;
            totalRotation *= Quaternion.Euler(tilt, 0f, 0f);
        }

        // WOBBLE
        if (enableWobble)
        {
            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleStrength;
            totalRotation *= Quaternion.Euler(0f, 0f, wobble);
        }

        // APPLY BASE ROTATION (tilt/wobble)
        transform.localRotation = totalRotation;

        // SPEED VARIATION
        if (enableSpeedVariation)
        {
            float variableSpeed = baseSpeed + Mathf.Sin(Time.time * 2f) * speedVariation;
            transform.position += transform.forward * variableSpeed * Time.deltaTime;
        }

        // STARTA TWIST OM DET ÄR DAGS
        if (enableRandomTwist && !isTwisting && Time.time >= nextTwistTime)
        {
            float twistAmount = Random.Range(-twistStrength, twistStrength);
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + twistAmount, 0f);
            isTwisting = true;
            twistTimer = 0f;
            nextTwistTime = Time.time + GetTwistInterval();
        }

        // UTFÖR TWISTEN GRADVIS
        if (isTwisting)
        {
            twistTimer += Time.deltaTime;
            float t = Mathf.Clamp01(twistTimer / twistDuration);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);

            if (t >= 1f)
            {
                isTwisting = false;
            }
        }
    }

    private float GetTwistInterval()
    {
        switch (twistFrequency)
        {
            case TwistFrequency.Fast:
                return 2f;
            case TwistFrequency.Slow:
                return 6f;
            default:
                return 4f;
        }
    }
}
