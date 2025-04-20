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

    private TextMeshProUGUI cueText;
    private Color originalTextColor;

    void Start()
    {
        cueText = visualCue.GetComponentInChildren<TextMeshProUGUI>();
        originalTextColor = cueText.color;

        visualCue.SetActive(false);
        StartCoroutine(PCSLoop());
    }

    private IEnumerator PCSLoop()
    {
        while (checksCompleted < totalChecks)
        {
            float waitTime = baseInterval + Random.Range(-surpriseOffset, surpriseOffset);
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(RunParticipationCheck());
        }

        yield return new WaitForSeconds(5f);
        ShowFinalResult();
    }

    private IEnumerator RunParticipationCheck()
    {
        cueActive = true;
        inputReceived = false;

        cueText.text = checkPromptText;
        cueText.color = originalTextColor;
        visualCue.SetActive(true);

        participationButton.action.performed += OnButtonPressed;
        participationButton.action.Enable();

        yield return new WaitForSeconds(cueDuration);

        cueActive = false;
        visualCue.SetActive(false);

        participationButton.action.performed -= OnButtonPressed;
        participationButton.action.Disable();

        if (inputReceived)
        {
            successCount++;
            Debug.Log("✅ PCS: Player participated.");
        }
        else
        {
            Debug.Log("❌ PCS: Player did NOT participate.");
        }

        checksCompleted++;
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        if (cueActive && !inputReceived)
        {
            inputReceived = true;

            // ✅ Visual feedback: text turns green
            cueText.color = Color.green;

            // ⏳ Wait 0.2s before hiding the cue
            StartCoroutine(DisableVisualAfterDelay(0.2f));
        }
    }

    private IEnumerator DisableVisualAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        visualCue.SetActive(false);
    }

    private void ShowFinalResult()
    {
        cueText.text = string.Format(scoreMessageFormat, successCount, totalChecks);
        cueText.color = originalTextColor;
        visualCue.SetActive(true);
    }
}
