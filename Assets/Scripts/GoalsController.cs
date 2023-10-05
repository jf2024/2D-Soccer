using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalsController : MonoBehaviour
{
    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;

    public Transform ballTransform;
    public Transform AwayBallPos;
    public Transform HomeBallPos;
    public Transform ballStartPos;
    public Transform p1;
    public Transform p2;

    private Vector3 startPos;
    private Vector3 p1Start;
    private Vector3 p2Start;

    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;

    AudioSource audioSource;

    void Start()
    {
        startPos = ballStartPos.position;
        p1Start = p1.position;
        p2Start = p2.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (transform.position.x >= 13.98 && transform.position.x <= 14.33)  // player1 scores
            {
                scorePlayer1++;
                score1.text = "P1: " + scorePlayer1;
                ballTransform.position = AwayBallPos.position;

                //reset position of players
                p1.position = p1Start;
                p2.position = p2Start;
            }
            else if (transform.position.x >= -14.33 && transform.position.x <= -13.98 ) // player2 scores
            {
                scorePlayer2++;
                score2.text = "P2: " + scorePlayer2;
                ballTransform.position = HomeBallPos.position;

                //reset position of players
                p2.position = p2Start;
                p1.position = p1Start;
            }

            //play audio
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip, 1f);

            // Reset ball velocity
            Rigidbody2D ballRigidbody = collision.GetComponent<Rigidbody2D>();
            ballRigidbody.velocity = Vector2.zero;
            ballRigidbody.angularVelocity = 0f;
        }
    }
}

