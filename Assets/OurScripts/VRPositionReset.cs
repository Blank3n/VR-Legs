using UnityEngine;

public class XRPositionReset : MonoBehaviour
{
    public Transform skateboard;  // The moving skateboard
    private Transform vrCamera;   // The Main Camera (VR headset)

    public float limitX = 0.5f;
    public float limitZ = 0.5f;
    
    void Start()
    {
        vrCamera = Camera.main?.transform;

        if (vrCamera == null)
        {
            Debug.LogError("No Main Camera found! Make sure the XR Rig has a Camera tagged as 'MainCamera'.");
            return;
        }

        if (skateboard == null)
        {
            Debug.LogError("Skateboard reference is missing! Assign the skateboard object in the inspector.");
            return;
        }
    }


    void LateUpdate()
    {
        if (vrCamera == null || skateboard == null) return;

        // Get camera's local position relative to XR Origin
        Vector3 cameraLocalPos = vrCamera.localPosition;
        Vector3 XRLocalPos = transform.localPosition;

        bool needsReset = false;

        // Correct X-axis
        if (Mathf.Abs(cameraLocalPos.x + XRLocalPos.x) > limitX)
        {
            Debug.LogError("x pos + pos = " + cameraLocalPos.x + " " + XRLocalPos.x);
            
            XRLocalPos.x = 0-cameraLocalPos.x;
            needsReset = true;
        }

        // Correct Z-axis
        if (Mathf.Abs(cameraLocalPos.z + XRLocalPos.z) > limitZ)
        {
            XRLocalPos.z = 0-cameraLocalPos.z;
            Debug.LogError("z pos + pos = " + cameraLocalPos.z + " " + XRLocalPos.z);
            needsReset = true;
        }

        // Apply correction (snap instantly)
        if (needsReset)
        {
            transform.localPosition = XRLocalPos;
        }
    }
}