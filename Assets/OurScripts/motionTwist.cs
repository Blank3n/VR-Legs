using UnityEngine;

public class motionTwist : MonoBehaviour
{
    public bool enableTwist = true;
    public float twistStrength = 10f;
    public float twistDuration = 1f;

    public enum TwistFrequency { Slow, Normal, Fast }
    public TwistFrequency twistFrequency = TwistFrequency.Normal;

    private float nextTwistTime = 0f;
    private Quaternion targetRotation;
    private bool isTwisting = false;
    private float twistTimer = 0f;

    void Start()
    {
        targetRotation = transform.rotation;
        nextTwistTime = Time.time + GetTwistInterval();
    }

    void Update()
    {
        if (enableTwist && !isTwisting && Time.time >= nextTwistTime)
        {
            float twist = Random.Range(-twistStrength, twistStrength);
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + twist, 0f);
            isTwisting = true;
            twistTimer = 0f;
            nextTwistTime = Time.time + GetTwistInterval();
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

    private float GetTwistInterval()
    {
        switch (twistFrequency)
        {
            case TwistFrequency.Fast: return 2f;
            case TwistFrequency.Slow: return 6f;
            default: return 4f;
        }
    }
}
