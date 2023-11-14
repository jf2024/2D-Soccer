using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;

    [SerializeField] private Transform ballTransform;
    [SerializeField] private Transform AwayBallPos;
    [SerializeField] private Transform HomeBallPos;
    [SerializeField] private Transform starterBallPos;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private TextMeshProUGUI score1;
    [SerializeField] private TextMeshProUGUI score2;

    [Header("Timer")]
    public float maxTime = 90.0f;
    private float timeLeft;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Game Over")]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private TextMeshProUGUI winnerText;

    [Header("Pause Menu")]
    [SerializeField] private Canvas pauseMenu;

    private Vector3 p1Start;
    private Vector3 p2Start;
    public bool hasPlayedStart = false;
    public bool hasPlayedEnd = false;

    public void nameS()
    {
        p1Start = p1.position;
        p2Start = p2.position;
    }
    private void Start()
    {

        nameS();

        scorePlayer1 = 0;
        scorePlayer2 = 0;
        timeLeft = maxTime;

        // Update UI
        UpdateScoreUI();

        StartGame();
    }


    private void Update()
    {
        if (hasPlayedStart)
        {
            UpdateTimer();

            if (timeLeft <= 0 && !hasPlayedEnd)
            {
                Time.timeScale = 0;
                hasPlayedEnd = true;

                DetermineWinner();

                if (gameOverCanvas != null)
                {
                    gameOverCanvas.enabled = true;
                }
            }
        }
    }

    private void UpdateTimer()
    {
        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(timeLeft, 0f);
        int minutes = Mathf.FloorToInt(timeLeft / 60.0f);
        int seconds = Mathf.FloorToInt(timeLeft % 60.0f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void GoalScored(int playerID)
    {
        if (playerID == 1)
        {
            scorePlayer1++;
            score1.text = "P1: " + scorePlayer1;
            ResetPosition();
        }
        else if (playerID == 2)
        {
            scorePlayer2++;
            score2.text = "P2: " + scorePlayer2;
            ResetPosition();
        }

        ResetBallVelocity();
    }

    public void DetermineWinner()
    {
        if (scorePlayer1 > scorePlayer2)
        {
            winnerText.text = "P1 wins!";
        }
        else if (scorePlayer2 > scorePlayer1)
        {
            winnerText.text = "P2 wins!";
        }
        else
        {
            winnerText.text = "Draw!";
        }
    }


    public void ResetPosition()
    {
        p1.position = p1Start;
        p2.position = p2Start;

        //PLAY AROUND WITH THESE TWO 

        //ballTransform.position = (scorePlayer1 > scorePlayer2) ? AwayBallPos.position : HomeBallPos.position;
        ballTransform.position = starterBallPos.position;
    }

    public void ResetBallVelocity()
    {
        Rigidbody2D ballRigidbody = ballTransform.GetComponent<Rigidbody2D>();
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.angularVelocity = 0f;
    }

    public void StartGame()
    {
        hasPlayedStart = true;
        timeLeft = maxTime;
        Time.timeScale = 1f;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = false;
        }

        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }

    // Pause the game
    public void PauseGame()
    {
        if (pauseMenu != null)
        {
            pauseMenu.enabled = true;
            Time.timeScale = 0f;
        }
    }

    // restart the game
    public void RestartGame()
    {
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        // Reset scores and timer
        gameOverCanvas.enabled = false;
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        timeLeft = maxTime;

        hasPlayedEnd = false;

        // Update UI
        UpdateScoreUI();

        // Reset positions and ball velocity
        ResetPosition();
        ResetBallVelocity();

        // Start the game again
        StartGame();
    }

    public void UpdateScoreUI()
    {
        score1.text = "P1: " + scorePlayer1;
        score2.text = "P2: " + scorePlayer2;
    }

}
