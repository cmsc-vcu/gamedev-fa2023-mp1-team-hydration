using UnityEngine;
using TMPro;

public class SpeedRunTimer : MonoBehaviour
{
    public TMP_Text timerText;  // Drag your TextMeshPro text here in the inspector

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int milliseconds = (int)((elapsedTime * 100) % 100);
        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
