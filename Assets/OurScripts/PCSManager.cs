using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PCSManager : MonoBehaviour
{
    [Header("Visual & Timing")]
    public GameObject visualCue;
    public float cueDuration = 3f;

    [Header("Check Frequency")]
    public float baseInterval = 20f;
    public float surpriseOffset = 5f;

    [Header("Check Settings")]
    public int totalChecks = 10;

    [Header("Anti-Spam Settings")]
    public int spamThreshold = 3;

    [Header("Input")]
    public InputActionReference participationButton;

    [Header("Prompt Texts")]
    [TextArea]
    public string checkPromptText = "PRESS A TO CONFIRM PARTICIPATION";

    [TextArea]
    public string scoreMessageFormat = "Score: {0} / {1}";

    private bool cueActive = false;
    private bool inputReceived = false;
    private int successCount = 0;
    private int checksCompleted = 0;
    private int spamCount = 0;
    private bool spammedThisCheck = false;

    private float compensationTime = 0;

    private TextMeshProUGUI cueText;
    private Color originalTextColor;

    private Coroutine pcsLoopCoroutine;
    private bool gameStarted = false;


    void Start()
    {
        cueText = visualCue.GetComponentInChildren<TextMeshProUGUI>();
        originalTextColor = cueText.color;

        visualCue.SetActive(false);

        participationButton.action.Enable(); // Ensure participation button is enabled from the start

        participationButton.action.performed += HandleButtonPress;
    }

    private void StartGame()
    {
        if (gameStarted) return; // Prevent starting the game loop again

        gameStarted = true;
        Debug.Log("PCSManager: Game started. Beginning PCS Loop. STARTGAME");
        pcsLoopCoroutine = StartCoroutine(PCSLoop());
    }

    private IEnumerator PCSLoop()
    {
        Debug.Log("PCSManager: Running PCS Loop.");

        while (checksCompleted < totalChecks)
        {
            float waitTime = baseInterval + Random.Range(-surpriseOffset, surpriseOffset);
            float compensationTime = -surpriseOffset;
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(RunParticipationCheck());
        }

        yield return new WaitForSeconds((5f + compensationTime));
        ShowFinalResult();
    }

    private IEnumerator RunParticipationCheck()
    {
        spammedThisCheck = spamCount >= spamThreshold;
        spamCount = 0;

        cueActive = true;
        inputReceived = false;

        cueText.text = checkPromptText;
        cueText.color = originalTextColor;
        visualCue.SetActive(true);

        yield return new WaitForSeconds(cueDuration);

        cueActive = false;

        if (inputReceived && !spammedThisCheck)
        {
            successCount++;
            Debug.Log("✅ PCS: Player participated.");
        }
        else
        {
            if (spammedThisCheck)
                Debug.Log("❌ PCS: Player failed due to spamming.");
            else
                Debug.Log("❌ PCS: Player did NOT participate.");

            cueText.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            visualCue.SetActive(false);
        }

        checksCompleted++;
    }

    private void HandleButtonPress(InputAction.CallbackContext context)
    {
        if (!cueActive)
        {
            spamCount++;
            Debug.Log("Button press detected OUTSIDE cue.");
        }
        else if (cueActive && !inputReceived)
        {
            if (!spammedThisCheck)
            {
                inputReceived = true;
                cueText.color = Color.green;
                Debug.Log("Button press detected DURING cue.");
                StartCoroutine(DisableVisualAfterDelay(0.2f));
            }
            else
            {
                Debug.Log("Ignored input: This check is failed due to spamming.");
            }
        }
    }

    private IEnumerator DisableVisualAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        visualCue.SetActive(false);
    }

    private void ShowFinalResult()
    {

        int vrLegsScore = FindObjectOfType<VRScore>().GetFinalScore();

        cueText.text = string.Format(scoreMessageFormat, successCount, totalChecks)
            + $"\nVR Legs Score: {vrLegsScore}";

        cueText.color = originalTextColor;
        visualCue.SetActive(true);
    }

    private void OnDestroy()
    {
        if (participationButton != null && participationButton.action != null)
        {
            participationButton.action.performed -= HandleButtonPress;
        }
    }

    public void StartGameFromManager()
    {
        if (gameStarted) return; // Prevent starting the game loop again

        gameStarted = true;
        Debug.Log("PCSManager: Game started externally. Beginning PCS Loop.");
        pcsLoopCoroutine = StartCoroutine(PCSLoop());
    }
}
