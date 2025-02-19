using UnityEngine;

public class TiltPlatform : MonoBehaviour
{
    public float tiltSpeed = 2f; // Hur snabbt plattformen tiltar
    public float maxTiltAngle = 20f; // Maxvinkel i grader
    public float changeInterval = 5f; // Hur ofta den byter riktning

    private float timeCounter = 0f;
    private Vector3 targetRotation;

    void Start()
    {
        SetRandomTargetRotation();
    }

    void Update()
    {
        timeCounter += Time.deltaTime;

        // Lerp för mjuk rörelse mot målvinkeln
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * tiltSpeed);

        // Byt riktning efter ett visst intervall
        if (timeCounter >= changeInterval)
        {
            SetRandomTargetRotation();
            timeCounter = 0f;
        }
    }

    void SetRandomTargetRotation()
    {
        float randomX = Random.Range(-maxTiltAngle, maxTiltAngle);
        float randomZ = Random.Range(-maxTiltAngle, maxTiltAngle);
        targetRotation = new Vector3(-10f, 0f, randomZ);
    }
}
