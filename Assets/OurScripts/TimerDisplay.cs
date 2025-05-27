using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public bool isRunning = false;

    private float elapsedTime = 0f;
    private static string previousTime; // persists across scene reloads

    void Update()
    {
        if (!isRunning)
        {
            // Show previous time only if it exists
            if (!string.IsNullOrEmpty(previousTime))
            {

                timerText.text = "Previous Time: " + previousTime + 
                "\nScore: " + PCSManager.finalScore + " / " + PCSManager.checkAmount;
                
            }
            return;
        }

        // Timer is running
        elapsedTime += Time.deltaTime;
        timerText.text = "Time: " + elapsedTime.ToString("F1") + " s";
    }

    private void OnDestroy()
    {
        // Save the last elapsed time when the object is destroyed
        previousTime = elapsedTime.ToString("F1") + " s";

    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f;
    }
}
