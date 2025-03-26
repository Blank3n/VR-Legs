using UnityEngine;

public class motionWobble : MonoBehaviour
{
    public bool enableWobble = true;
    public float wobbleStrength = 5f;
    public float wobbleSpeed = 5f;

    void Update()
    {
        if (!enableWobble) return;

        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleStrength;
        transform.localRotation = Quaternion.Euler(0f, 0f, wobble);
    }
}
