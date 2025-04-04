using UnityEngine;

public class motionLag : MonoBehaviour
{
    [Header("Aktivera hackande FPS")]
    public bool enableLag = true;

    [Header("FPS-inställningar")]
    public int targetFPS = 15; // Låg FPS-nivå vid lagg
    public float timeBetweenJumps = 5f; // Hur ofta vi växlar FPS

    private float nextToggleTime;
    private bool isLagging = false;

    void Start()
    {
        nextToggleTime = Time.time + timeBetweenJumps;
    }

    void Update()
    {
        if (!enableLag) return;

        if (Time.time >= nextToggleTime)
        {
            isLagging = !isLagging;
            Application.targetFrameRate = isLagging ? targetFPS : 90;
            nextToggleTime = Time.time + timeBetweenJumps;
        }
    }

    void OnDisable()
    {
        Application.targetFrameRate = 90; // Säkerställ normal FPS om script stängs av
    }
}
