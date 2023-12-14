using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post : MonoBehaviour
{
    public float slowDownFactor = 0.8f;     //dead ball power up can be 0.2
    public float deadBall = 0.1f;
    public float push = 1.5f;

    private Rigidbody2D collidedBall = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            collidedBall = ballRigidbody;
            ballRigidbody.velocity *= slowDownFactor;
        }
    }

    private void FixedUpdate()
    {
        if (collidedBall != null)
        {
            if (collidedBall.velocity.magnitude < deadBall)
            {
                Vector2 forceDirection = (transform.position - collidedBall.transform.position).normalized;
                collidedBall.AddForce(forceDirection * push, ForceMode2D.Impulse);
            }
        }
    }
}
