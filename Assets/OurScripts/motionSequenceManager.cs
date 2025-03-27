using UnityEngine;

public class MotionSequenceManager : MonoBehaviour
{
    public motionTilt tiltScript;
    public motionWobble wobbleScript;
    public motionTwist twistScript;

    public float tiltStartTime = 10f;
    public float wobbleStartTime = 20f;
    public float twistStartTime = 30f;
    public float escalateTime = 45f;

    private float timer = 0f;

    void Start()
    {
        // Inaktivera allt i början
        tiltScript.enableTilt = false;
        wobbleScript.enableWobble = false;
        twistScript.enableTwist = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tiltStartTime && !tiltScript.enableTilt)
            tiltScript.enableTilt = true;

        if (timer >= wobbleStartTime && !wobbleScript.enableWobble)
            wobbleScript.enableWobble = true;

        if (timer >= twistStartTime && !twistScript.enableTwist)
            twistScript.enableTwist = true;

        if (timer >= escalateTime)
        {
            // Öka styrkan gradvis efter tid
            tiltScript.tiltStrength = Mathf.Lerp(10f, 30f, (timer - escalateTime) / 30f);
            wobbleScript.wobbleStrength = Mathf.Lerp(5f, 25f, (timer - escalateTime) / 30f);
            twistScript.twistStrength = Mathf.Lerp(10f, 45f, (timer - escalateTime) / 30f);
            twistScript.twistInterval = Mathf.Lerp(6f, 2f, (timer - escalateTime) / 30f); // snabbare twist
        }
    }
}
