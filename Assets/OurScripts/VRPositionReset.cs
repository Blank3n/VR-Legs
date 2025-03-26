using UnityEngine;

public class XRPositionReset : MonoBehaviour
{
    public Transform skateboard;  // The moving skateboard
    private Transform vrCamera;   // The Main Camera (VR headset)

    // Set movement limits (player should stay within -0.5 to 0.5 on X and Z)
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

        // Calculate the combined world position of Main Camera and XR Origin
        Vector3 combinedPosition = vrCamera.position + transform.position;
        Vector3 xrOriginPosition = transform.position;

        bool needsReset = false;

        // Check if outside X bounds
        if (Mathf.Abs(combinedPosition.x) > limitX)
        {
            xrOriginPosition.x += Mathf.Sign(combinedPosition.x) * limitX;
            needsReset = true;
        }

        // Check if outside Z bounds
        if (Mathf.Abs(combinedPosition.z) > limitZ)
        {
            xrOriginPosition.z += Mathf.Sign(combinedPosition.z) * limitZ;
            needsReset = true;
        }

        // Apply correction if needed
        if (needsReset)
        {
            transform.position = xrOriginPosition;
        }
    }
}
