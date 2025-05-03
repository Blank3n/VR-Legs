using UnityEngine;

public class motionTwist : MonoBehaviour
{
    public bool enableTwist = true;
    public float twistStrength = 10f;
    public float twistDuration = 1f;
    public float twistInterval = 4f; // Justerbar i Inspector

    private float nextTwistTime = 0f;
    private Quaternion targetRotation;
    private bool isTwisting = false;
    private float twistTimer = 0f;

    void Start()
    {
        targetRotation = transform.rotation;
        nextTwistTime = Time.time + twistInterval;
    }

    void Update()
    {   
        if (!enableTwist)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            return;
        }

        if (enableTwist && !isTwisting && Time.time >= nextTwistTime)
        {
            float twist = Random.Range(-twistStrength, twistStrength);
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + twist, 0f);
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
}
