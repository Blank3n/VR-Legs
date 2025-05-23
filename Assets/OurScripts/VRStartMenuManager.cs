using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // For UI Text
using TMPro; // Optional, if using TextMeshPro

public class VRStartMenuManager : MonoBehaviour
{
    [Header("UI & Experience")]
    public GameObject startMenuCanvas;              // The menu canvas to hide
    public MonoBehaviour[] experienceScripts;       // Scripts to enable when Play is pressed

    [Header("Input")]
    public InputActionReference startButtonAction;  // Reference to an Input Action (e.g. trigger or A)
    public InputActionReference participationButtonAction; // Reference to participation button (A)

    public TextMeshProUGUI startMenuText; // Assign in inspector if using TextMeshPro
    // public Text startMenuText; // Use this instead if using legacy UnityEngine.UI.Text

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

        FindObjectOfType<TimerDisplay>()?.StopTimer(); // Makes sure timer doesn't run in the StartMenu
    }

    private void OnStartButtonPressed(InputAction.CallbackContext context)
    {
        if (hasStarted)
            return;

        Debug.Log("▶️ Start button pressed. Starting experience.");

        foreach (var script in experienceScripts)
        {
            script.enabled = true;
        }

        PCSManager pcsManager = FindObjectOfType<PCSManager>();
        if (pcsManager != null)
        {
            pcsManager.StartGameFromManager();
        }

        if (participationButtonAction != null)
        {
            participationButtonAction.action.Enable();
        }

        FindObjectOfType<TimerDisplay>()?.StartTimer(); // Start timer

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
            participationButtonAction.action.Disable();
        }
    }
}
