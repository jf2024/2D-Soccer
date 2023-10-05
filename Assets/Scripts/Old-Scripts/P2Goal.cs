using UnityEngine;
using TMPro;

public class P2Goal : MonoBehaviour
{
    public int scorePlayer2 = 0;
    public Transform ballTransform;
    public Transform p2;
    public Transform p1;
    private Vector3 startingPosition;
    private Vector3 p2Start;
    private Vector3 p1Start;

    //starts with left side player 
    public Transform HomeBallPos;
    public Transform startPos;

    public TextMeshProUGUI score2;

    AudioSource audioSource;

    private void Start()
    {
        startingPosition = startPos.position;
        p2Start = p2.position;
        p1Start = p1.position;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("enter collision");
        if (other.gameObject.CompareTag("Ball"))
        {
            //Debug.Log(transform.position.x);
            if (transform.position.x >= -14.33) // Player 2 scores
            {
                //Debug.Log("enter x");
                scorePlayer2++;
                score2.text = "P2: " + scorePlayer2;

                ballTransform.position = HomeBallPos.position;
                p2.position = p2Start;
                p1.position = p1Start;
                //sound here
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
