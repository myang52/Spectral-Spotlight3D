using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float remainingTime = 300f; // 5 minutes in seconds
    private bool gameOverTriggered = false; // Flag to prevent multiple triggers

    void Update()
    {
        // Decrease the remaining time
        remainingTime -= Time.deltaTime;

        // Ensure the time does not go below zero
        remainingTime = Mathf.Max(remainingTime, 0);

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        // Format and display the timer text
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Check if the timer has reached zero
        if (remainingTime == 0 && !gameOverTriggered)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverTriggered = true;
            SceneManager.LoadScene("GameOver");
            
        }
    }
}
