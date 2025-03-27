using UnityEngine;

public class motionLag : MonoBehaviour
{
    [Header("Aktivera hackande FPS")]
    public bool enableLag = true;

    [Header("FPS-inställningar")]
    public int targetFPS = 15; // t.ex. 15 för rejält ryckigt
    public float timeBetweenJumps = 5f; // hur ofta vi växlar FPS

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
            Application.targetFrameRate = isLagging ? targetFPS : 90; // 90 för VR
            nextToggleTime = Time.time + timeBetweenJumps;
        }
    }
}
