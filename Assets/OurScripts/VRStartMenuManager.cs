using UnityEngine;
using UnityEngine.InputSystem;

public class VRStartMenuManager : MonoBehaviour
{
    [Header("UI & Experience")]
    public GameObject startMenuCanvas;              // The menu canvas to hide
    public MonoBehaviour[] experienceScripts;       // Scripts to enable when Play is pressed

    [Header("Input")]
    public InputActionReference startButtonAction;  // Reference to an Input Action (e.g. trigger or A)
    public InputActionReference participationButtonAction; // Reference to participation button (A)

    private bool hasStarted = false;

    private void Start()
    {
        // Disable gameplay/experience logic until input is received
        foreach (var script in experienceScripts)
        {
            script.enabled = false;
        }

        startMenuCanvas.SetActive(true);

        if (startButtonAction != null)
        {
            startButtonAction.action.Enable();
            startButtonAction.action.performed += OnStartButtonPressed;
        }

        if (participationButtonAction != null)
        {
            participationButtonAction.action.Enable(); // Ensure it's enabled from the start
        }
        
        FindObjectOfType<TimerDisplay>()?.StopTimer();
    }

    private void OnStartButtonPressed(InputAction.CallbackContext context)
    {
        if (hasStarted)
            return;

        Debug.Log("▶️ Start button pressed. Starting experience.");

        // Enable gameplay/experience
        foreach (var script in experienceScripts)
        {
            script.enabled = true;
        }

        // Call the StartGameFromManager method on PCSManager to begin the PCS check loop
        PCSManager pcsManager = FindObjectOfType<PCSManager>();
        if (pcsManager != null)
        {
            pcsManager.StartGameFromManager(); // This will start PCSManager's check loop
        }

        // Ensure participation button input action is enabled for the duration of the game
        if (participationButtonAction != null)
        {
            participationButtonAction.action.Enable(); // Re-enable participation button
        }
        FindObjectOfType<TimerDisplay>()?.StartTimer(); // ⏱️ Starta timern

        // Hide the menu
        startMenuCanvas.SetActive(false);
        hasStarted = true;
    }

    private void OnDestroy()
    {
        if (startButtonAction != null && startButtonAction.action != null)
        {
            startButtonAction.action.performed -= OnStartButtonPressed;
        }

        if (participationButtonAction != null && participationButtonAction.action != null)
        {
            participationButtonAction.action.Disable(); // Disable if necessary when done
        }
    }
}
