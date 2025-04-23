using UnityEngine;

public class LightFader : MonoBehaviour
{
    public float targetIntensity = 1f;
    public float fadeSpeed = 2f;

    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        if (myLight == null) return;

        myLight.intensity = Mathf.Lerp(myLight.intensity, targetIntensity, Time.deltaTime * fadeSpeed);
    }

    public void SetTargetIntensity(float value)
    {
        targetIntensity = value;
    }
}
