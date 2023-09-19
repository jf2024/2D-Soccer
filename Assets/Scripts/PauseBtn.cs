using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseBtn : MonoBehaviour
{
    public GameObject pauseMenu;
    public TextMeshProUGUI countdownText;

    private bool isPaused = false;
    private float countdownTimer = 3f;


    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            countdownTimer -= Time.unscaledDeltaTime;
            if (countdownTimer <= 0f)
            {
                ResumeGame();
            }
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    public void PauseGameplay()
    {
        isPaused = !isPaused; // Toggle pause state
    }

    public void ResumeGame()
    {
        isPaused = false;
    }
}
