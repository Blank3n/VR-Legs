using UnityEngine;

public class motionTesterVR : MonoBehaviour
{
    [Header("Tilt Settings")]
    public bool enableTilt = false;
    public float tiltBaseStrength = 10f;
    public float tiltBaseSpeed = 1.5f;
    public float tiltStrengthVariation = 5f;
    public float tiltSpeedVariation = 0.5f;
    public float tiltChangeInterval = 3f;

    [Header("Wobble Settings")]
    public bool enableWobble = false;
    public float wobbleBaseStrength = 5f;
    public float wobbleBaseSpeed = 5f;
    public float wobbleStrengthVariation = 3f;
    public float wobbleSpeedVariation = 1f;
    public float wobbleChangeInterval = 3f;

    [Header("Jitter (slumpmässiga ryck)")]
    public bool enableJitter = false;
    public float jitterAmount = 0.02f;

    [Header("Zoom Pulsing")]
    public bool enableZoom = false;
    public float zoomSpeed = 2f;
    public float zoomStrength = 0.05f;
    private Vector3 originalScale;

    [Header("Drift")]
    public bool enableDrift = false;
    public Vector3 driftDirection = new Vector3(0.1f, 0, 0);
    public float driftSpeed = 0.5f;

    [Header("Vertical Bobbing")]
    public bool enableBobbing = false;
    public float bobbingHeight = 0.1f;
    public float bobbingSpeed = 1f;
    private float baseY;

    [Header("Twist")]
    public bool enableRandomTwist = false;
    public float twistStrength = 90f;
    public float twistDuration = 1f;
    public float twistInterval = 4f;


    // Internal runtime values
    private float tiltTimer, wobbleTimer;
    private float currentTiltStrength, currentTiltSpeed, tiltPhaseOffset;
    private float currentWobbleStrength, currentWobbleSpeed, wobblePhaseOffset;
    private float nextTwistTime = 0f;
    private Quaternion targetRotation;
    private bool isTwisting = false;
    private float twistTimer = 0f;

    void Start()
    {


        RandomizeTilt();
        RandomizeWobble();
    }

    void Update()
    {
    Quaternion rotation = Quaternion.identity;

    // Tilt
    if (enableTilt)
    {
        tiltTimer += Time.deltaTime;
        if (tiltTimer >= tiltChangeInterval)
        {
            RandomizeTilt();
            tiltTimer = 0f;
        }

        float tilt = Mathf.Sin(Time.time * currentTiltSpeed + tiltPhaseOffset) * currentTiltStrength;
        rotation *= Quaternion.Euler(0f, 0f, tilt);
    }

    // Wobble
    if (enableWobble)
    {
        wobbleTimer += Time.deltaTime;
        if (wobbleTimer >= wobbleChangeInterval)
        {
            RandomizeWobble();
            wobbleTimer = 0f;
        }

        float wobble = Mathf.Sin(Time.time * currentWobbleSpeed + wobblePhaseOffset) * currentWobbleStrength;
        rotation *= Quaternion.Euler(wobble, 0f, 0f);
    }

    transform.localRotation = rotation;

        // Apply rotation
        transform.localRotation = rotation;

        // Jitter
        if (enableJitter)
        {
            Vector3 jitter = new Vector3(
                Random.Range(-jitterAmount, jitterAmount),
                Random.Range(-jitterAmount, jitterAmount),
                Random.Range(-jitterAmount, jitterAmount)
            );
            transform.localPosition += jitter;
        }

        // Drift
        if (enableDrift)
        {
            transform.position += driftDirection.normalized * driftSpeed * Time.deltaTime;
        }

        // Bobbing
        if (enableBobbing)
        {
            float offsetY = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
            Vector3 pos = transform.position;
            pos.y = baseY + offsetY;
            transform.position = pos;
        }

        // Zoom pulsing
        if (enableZoom)
        {
            float scaleOffset = Mathf.Sin(Time.time * zoomSpeed) * zoomStrength;
            transform.localScale = originalScale + Vector3.one * scaleOffset;
        }

        // Twist
        if (enableRandomTwist && !isTwisting && Time.time >= nextTwistTime)
        {
            float twistAmount = Random.Range(-twistStrength, twistStrength);
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + twistAmount, 0f);
            isTwisting = true;
            twistTimer = 0f;
            nextTwistTime = Time.time + twistInterval;
        }

        if (isTwisting)
        {
            twistTimer += Time.deltaTime;
            float t = Mathf.Clamp01(twistTimer / twistDuration);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);

            if (t >= 1f)
                isTwisting = false;
        }
    }

    void RandomizeTilt()
    {
        currentTiltSpeed = Random.Range(0.5f, 2.5f);
        currentTiltStrength = Random.Range(5f, 20f);
        tiltPhaseOffset = Random.Range(0f, Mathf.PI * 2);
        tiltChangeInterval = Random.Range(2f, 6f);
        tiltTimer = 0f;
    }

    void RandomizeWobble()
    {
        currentWobbleSpeed = Random.Range(1f, 3f);
        currentWobbleStrength = Random.Range(5f, 15f);
        wobblePhaseOffset = Random.Range(0f, Mathf.PI * 2);
        wobbleChangeInterval = Random.Range(2f, 6f);
        wobbleTimer = 0f;
    }
}
