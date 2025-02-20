using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // For XR Input
using UnityEngine.InputSystem; // For new Input System

public class XRShooting : MonoBehaviour
{
    public Transform shootOrigin; // Where the raycast originates (controller)
    public float shootDistance = 50f; // Max shooting distance
    public LayerMask targetLayer; // Layer to detect balls
    public GameObject muzzleFlashPrefab; // Optional muzzle flash effect
    public InputActionProperty shootAction; // Input system action for shooting

    void Update()
    {
        // Check if the trigger button is pressed using the new Input System
        if (shootAction.action.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("Did Shoot");

        Ray ray = new Ray(shootOrigin.position, shootOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootDistance, targetLayer))
        {
            Debug.Log("Träffade: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("Ball"))
            {
                hit.collider.GetComponent<FloatingMotion>().HideBall(); // Hide the ball
                /*
                
                // Use FindFirstObjectByType instead of FindObjectOfType
                ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddPoint(); // Add score if ScoreManager exists
                }
                else
                {
                    Debug.LogWarning("ScoreManager not found!");
                }
                */
            }
        }

        // Create a muzzle flash effect (optional)
        if (muzzleFlashPrefab)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, shootOrigin.position, Quaternion.identity);
            Destroy(flash, 0.2f);
        }
    }
}
