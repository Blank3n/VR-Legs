using UnityEngine;
using System.Collections;

public class FloatingMotion : MonoBehaviour
{
    public float floatSpeed = 1f;  // Speed of up and down motion
    public float floatHeight = 5f; // Height variation on Y-axis
    public float orbitSpeed = 500f; // Speed of orbiting around player
    public float orbitRadius = 3000f; // Distance from player
    public float respawnTime = 3f; // Time before reappearing

    private Vector3 playerPos = new Vector3(0, 30, 0); // Player's fixed position
    private MeshRenderer meshRenderer;
    private Collider ballCollider;
    private float angle; // Angle for orbit calculation

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        ballCollider = GetComponent<Collider>();

        // Initialize angle based on current position
        Vector3 offset = transform.position - playerPos;
        angle = Mathf.Atan2(offset.z, offset.x); // Set angle based on initial position
    }

    void Update()
    {
        // Increase angle over time to rotate around the player
        angle += orbitSpeed * Time.deltaTime;

        // Calculate new X and Z positions based on orbitRadius
        float x = playerPos.x + Mathf.Cos(angle) * orbitRadius;
        float z = playerPos.z + Mathf.Sin(angle) * orbitRadius;

        // Floating motion on the Y-axis
        float y = playerPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Apply new position
        transform.position = new Vector3(x, y, z);
    }

    public void HideBall()
    {
        meshRenderer.enabled = false;
        ballCollider.enabled = false;
        StartCoroutine(RespawnBall());
    }

    IEnumerator RespawnBall()
    {
        yield return new WaitForSeconds(respawnTime);
        meshRenderer.enabled = true;
        ballCollider.enabled = true;
    }
}
