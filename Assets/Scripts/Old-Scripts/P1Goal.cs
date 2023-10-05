using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class P1Goal : MonoBehaviour
{
    public int scorePlayer1 = 0;
    public Transform ballTransform;
    public Transform AwayBallPos;
    public Transform ballStartPos;
    public Transform p1;
    public Transform p2;

    private Vector3 startPos;
    private Vector3 p1Start;
    private Vector3 p2Start;

    public TextMeshProUGUI score1;

    AudioSource audioSource;

    void Start()
    {
        startPos = ballStartPos.position;
        p1Start = p1.position;
        p2Start = p2.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("enter collision");
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log(transform.position.x);
            if (transform.position.x <= 14.33) // Player 1 scores
            {
                //Debug.Log("enter x");
                scorePlayer1++;
                score1.text = "P1: " + scorePlayer1;

                ballTransform.position = AwayBallPos.position;
                p1.position = p1Start;
                p2.position = p2Start;

                audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip, 1f);

            }

            // Reset ball velocity
            Rigidbody2D ballRigidbody = other.GetComponent<Rigidbody2D>();
            ballRigidbody.velocity = Vector2.zero;
            ballRigidbody.angularVelocity = 0f;
        }
    }


}
