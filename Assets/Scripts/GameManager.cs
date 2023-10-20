using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;

    public Transform ballTransform;      
    public Transform AwayBallPos;        
    public Transform HomeBallPos;        
    public Transform p1;                 
    public Transform p2;                 
    public Transform ballStartPos;       
    private Vector3 p1Start;
    private Vector3 p2Start;
    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;

    public float timeLeft = 90.0f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winnerText;
    public Canvas gameOverCanvas;

    AudioSource audioSource;
    public AudioSource startWhis;
    public AudioSource endWhis;
    private bool hasPlayedStart = false;
    private bool hasPlayedEnd = false;

    public PlayerStats p1stats = new PlayerStats();
    public PlayerStats p2stats = new PlayerStats();

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        p1Start = p1.position;
        p2Start = p2.position;
        startWhis.PlayOneShot(startWhis.clip, 1f);
    }

    private void Update()
    {
        if (!hasPlayedStart)
        {
            startWhis.PlayOneShot(startWhis.clip, 1f);
            hasPlayedStart = true;
        }

        timeLeft -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeLeft / 60.0f);
        int seconds = Mathf.FloorToInt(timeLeft % 60.0f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeLeft <= 0)
        {
            Time.timeScale = 0;

            if (!hasPlayedEnd)
            {
                endWhis.PlayOneShot(endWhis.clip, 1f);
                hasPlayedEnd = true;
            }

            timerText.text = "00:00";

            if (gameOverCanvas != null)
            {
                DetermineWinner();
                gameOverCanvas.enabled = true;
            }
        }
    }

    public void GoalScored(int playerID)
    {
        if (playerID == 1) // Player 1 scores
        {
            scorePlayer1++;
            score1.text = "P1: " + scorePlayer1;
            ballTransform.position = AwayBallPos.position;
            ResetPosition();
        }
        else if (playerID == 2) // Player 2 scores
        {
            scorePlayer2++;
            score2.text = "P2: " + scorePlayer2;
            ballTransform.position = HomeBallPos.position;
            ResetPosition();
        }

        // Play audio
        audioSource.PlayOneShot(audioSource.clip, 1f);

        ResetBallVelocity();
    }

    public void DetermineWinner()
    {
        if (scorePlayer1 > scorePlayer2) {
            winnerText.text = "P1 wins!";
            GameManager.Instance.p1stats.wins++;
            GameManager.Instance.p2stats.loss++;
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

    private void ResetPosition()
    {
        // Reset position of players
        p2.position = p2Start;
        p1.position = p1Start;
    }

    private void ResetBallVelocity()
    {
        Rigidbody2D ballRigidbody = ballTransform.GetComponent<Rigidbody2D>();
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.angularVelocity = 0f;
    }

    public void getPlayerStats(int pid)
    {

        PlayerStats pstats = pid == 1 ? p1stats : p2stats;

        Debug.Log("p" + pid + "stats");

        Debug.Log(pstats.wins);
        Debug.Log(pstats.loss);
        Debug.Log(pstats.draw);

    }
}
