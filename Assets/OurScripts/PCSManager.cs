using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PCSManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource successAudio;
    public AudioSource failAudio;

    [Header("Visual & Timing")]
    public GameObject visualCue;  // Parent Canvas or Panel
    public float cueDuration = 3f;

    [Header("Cue Image")]
    public GameObject cuePanel;
    public Image imageCue;           // The UI Image object to show
    public Sprite cueSprite;         // The single image/sprite to display
    public Vector2 cueSize = new Vector2(200, 200); // Size in pixels

    [Header("Check Frequency")]
    public float baseInterval = 20f;
    public float surpriseOffset = 5f;

    [Header("Check Settings")]
    public int totalChecks = 10;
    public int spamThreshold = 3;

    [Header("Reset Settings")]
    public float resetDelay = 5f;

    [Header("Input")]
    public InputActionReference participationButton;
    public InputActionReference PanicButton;

    [Header("Prompt Texts")]
    [TextArea]
    public string checkPromptText = "PRESS A TO CONFIRM PARTICIPATION";

    [TextArea]
    public string scoreMessageFormat = "Score: {0} / {1}";

    private float tmpOffset = 0;
    private bool isEnding = false;
    private Coroutine runningCheck;
    private bool cueActive = false;
    private bool inputReceived = false;
    private int successCount = 0;
    private int spamCount = 0;
    private bool spammedThisCheck = false;

    private Coroutine pcsLoopCoroutine;
    private bool gameStarted = false;

    private TextMeshProUGUI cueText;
    private Color originalTextColor;

    private readonly Vector2[] screenCorners = new Vector2[4]
    {
        new Vector2(0, 1), // Top-left
        new Vector2(1, 1), // Top-right
        new Vector2(0, 0), // Bottom-left
        new Vector2(1, 0)  // Bottom-right
    };

    void Start()
    {
        cueText = visualCue.GetComponentInChildren<TextMeshProUGUI>();
        if (cueText != null)
        {
            originalTextColor = cueText.color;
            cueText.gameObject.SetActive(false);
        }

        cuePanel.SetActive(false);
        visualCue.SetActive(false);

        if (imageCue != null)
        {
            imageCue.enabled = false;
            imageCue.sprite = cueSprite;
        }

        participationButton.action.Enable();
        participationButton.action.performed += HandleButtonPress;

        PanicButton.action.Enable();
        PanicButton.action.performed += OnResetInput;
    }

    private void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;
        pcsLoopCoroutine = StartCoroutine(PCSLoop());
    }

    private IEnumerator PCSLoop()
    {
        for (int i = 0; i < totalChecks; i++)
        {
            float waitTime = baseInterval;
            yield return new WaitForSeconds(waitTime);
            runningCheck = StartCoroutine(RunParticipationCheck());
        }

        yield return new WaitForSeconds(5f - tmpOffset);
        ShowFinalResult();
    }

    private IEnumerator RunParticipationCheck()
    {
        tmpOffset = Random.Range(-surpriseOffset, surpriseOffset);
        float waitTime = tmpOffset;
        yield return new WaitForSeconds(waitTime);

        spammedThisCheck = spamCount >= spamThreshold;
        spamCount = 0;

        cueActive = true;
        inputReceived = false;

        visualCue.SetActive(true);

        // Show image cue at random position
        if (imageCue != null && cueSprite != null)
        {
            imageCue.sprite = cueSprite;
            imageCue.rectTransform.sizeDelta = cueSize;

            Vector2 anchor = screenCorners[Random.Range(0, screenCorners.Length)];
            imageCue.rectTransform.anchorMin = anchor;
            imageCue.rectTransform.anchorMax = anchor;
            imageCue.rectTransform.pivot = anchor;
            imageCue.rectTransform.anchoredPosition = Vector2.zero;
            imageCue.enabled = true;
        }

        yield return new WaitForSeconds(cueDuration);

        cueActive = false;

        if (inputReceived && !spammedThisCheck)
        {
            successCount++;
            // Already handled in input
        }
        else
        {
            if (failAudio != null) failAudio.Play();
            if (imageCue != null) imageCue.enabled = false;
            visualCue.SetActive(false);
        }
    }

    private void HandleButtonPress(InputAction.CallbackContext context)
    {
        if (!cueActive)
        {
            spamCount++;
        }
        else if (cueActive && !inputReceived)
        {
            if (!spammedThisCheck)
            {
                inputReceived = true;
                if (successAudio != null) successAudio.Play();
                StartCoroutine(DisableVisualAfterDelay(0.2f));
            }
        }
    }

    private IEnumerator DisableVisualAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (imageCue != null)
            imageCue.enabled = false;

        visualCue.SetActive(false);
    }

    private void ShowFinalResult()
    {
        int vrLegsScore = FindObjectOfType<VRScore>().GetFinalScore();

        if (cueText != null)
        {
            cueText.text = string.Format(scoreMessageFormat, successCount, totalChecks)
                + $"\nVR Legs Score: {vrLegsScore}";
            cueText.color = originalTextColor;
            cueText.gameObject.SetActive(true);
        }

        visualCue.SetActive(true);
        cuePanel.SetActive(true);
    }

    private void OnDestroy()
    {
        if (participationButton?.action != null)
            participationButton.action.performed -= HandleButtonPress;

        if (PanicButton?.action != null)
            PanicButton.action.performed -= OnResetInput;
    }

    public void StartGameFromManager()
    {
        if (gameStarted) return;
        gameStarted = true;
        pcsLoopCoroutine = StartCoroutine(PCSLoop());
    }

    private void OnResetInput(InputAction.CallbackContext context)
    {
        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame()
    {
        isEnding = true;
        ShowFinalResult();

        if (runningCheck != null)
            StopCoroutine(runningCheck);
        if (pcsLoopCoroutine != null)
            StopCoroutine(pcsLoopCoroutine);

        yield return new WaitForSeconds(resetDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
