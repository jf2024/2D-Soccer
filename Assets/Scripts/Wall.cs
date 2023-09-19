using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wall : MonoBehaviour
{
    public float slowDownFactor = 0.9f; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            ballRigidbody.velocity *= slowDownFactor;
        }
    }
}
