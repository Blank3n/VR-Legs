using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VRScore : MonoBehaviour
{
    public int maxScore = 100;
    private int currentScore;

    public Key illamåendeKey = Key.B;

    void Start()
    {
        currentScore = maxScore;
        if (resultPanel != null)
            resultPanel.SetActive(false); // Dölj resultatpanelen från start
    }

    void Update()
    {
        if (Keyboard.current[illamåendeKey].wasPressedThisFrame)
        {
            // Om knappen trycks ner, minska poängen
            Debug.Log("Illamående key pressed. Current score: " + currentScore);

            currentScore = Mathf.Max(0, currentScore - 1);
        }
    }

    public void ShowFinalScore()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Final VR Legs Score: " + currentScore;
    }

    public int GetFinalScore()
    {
        return currentScore;
    }
}
