using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post : MonoBehaviour
{
    public float slowDownFactor = 2.0f;     //dead ball power up can be 0.2

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            ballRigidbody.velocity *= slowDownFactor;
        }
    }
}
