using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public bool isRunning = false;

    private float elapsedTime = 0f;

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        timerText.text = "Time: " + elapsedTime.ToString("F1") + " s";
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
