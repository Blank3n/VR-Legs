using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ActivePlayerCheckSystemBothControllers : MonoBehaviour
{
    public Canvas promptCanvas; // Canvas to display the prompt
    public Image promptImage; // Image to show the direction (e.g., arrow)
    public Sprite[] directionSprites; // Sprites for up, down, left, right
    public float promptDuration = 5f; // Time to respond to the prompt
    public float throwThreshold = 2f; // Minimum velocity to consider a "throw"

    private Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
    private int currentDirectionIndex = -1;
    private float promptTimer;
    private bool isPromptActive = false;

    private Vector3 lastLeftControllerVelocity;
    private Vector3 lastRightControllerVelocity;

    void Start()
    {
        promptCanvas.enabled = false; // Hide the prompt initially
    }

    void Update()
    {
        // Check if a prompt is active
        if (isPromptActive)
        {
            promptTimer -= Time.deltaTime;

            // If the player fails to respond in time
            if (promptTimer <= 0)
            {
                FailResponse();
                return;
            }

            // Check for controller movement on both controllers
            if (CheckControllerMovement(XRNode.LeftHand, lastLeftControllerVelocity, out lastLeftControllerVelocity) ||
                CheckControllerMovement(XRNode.RightHand, lastRightControllerVelocity, out lastRightControllerVelocity))
            {
                SuccessResponse();
            }
        }
        else
        {
            // Randomly trigger a prompt (for testing, you can replace this with a condition)
            if (Input.GetKeyDown(KeyCode.Space)) // Replace with your condition
            {
                StartPrompt();
            }
        }
    }

    void StartPrompt()
    {
        // Randomly choose a direction
        currentDirectionIndex = Random.Range(0, directions.Length);
        promptImage.sprite = directionSprites[currentDirectionIndex]; // Set the arrow sprite
        promptCanvas.enabled = true; // Show the prompt
        promptTimer = promptDuration; // Reset the timer
        isPromptActive = true;
    }

    bool CheckControllerMovement(XRNode controllerNode, Vector3 lastVelocity, out Vector3 newVelocity)
    {
        newVelocity = Vector3.zero;

        // Get the controller's velocity
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        Vector3 controllerVelocity;
        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerVelocity))
        {
            // Calculate the change in velocity (acceleration)
            Vector3 acceleration = controllerVelocity - lastVelocity;
            newVelocity = controllerVelocity;

            // Check if the player moved the controller in the correct direction with enough force
            if (acceleration.magnitude > throwThreshold)
            {
                Vector3 movementDirection = acceleration.normalized;
                Vector3 expectedDirection = directions[currentDirectionIndex];

                // Check if the movement direction matches the expected direction
                if (Vector3.Dot(movementDirection, expectedDirection) > 0.7f) // Allow some leeway
                {
                    return true; // Valid movement detected
                }
            }
        }

        return false; // No valid movement detected
    }

    void SuccessResponse()
    {
        Debug.Log("Player responded correctly!");
        promptCanvas.enabled = false; // Hide the prompt
        isPromptActive = false;
        // Reward the player (e.g., add points, continue the game)
    }

    void FailResponse()
    {
        Debug.Log("Player failed to respond!");
        promptCanvas.enabled = false; // Hide the prompt
        isPromptActive = false;
        // Penalize the player (e.g., reduce health, end the game)
    }
}