using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 50.0f;

    public float jumpForceDefault = 50.0f;
    public float jumpForceBoosted = 70.0f;
    public float jumpForceBstTime = 3.5f;

    public float speedDefault = 5.0f;
    public float speedBoost = 7.0f;
    public float speedBstTime = 3.5f;

    public float speedDecrease = 3.5f;
    public float jumpDecrease = 40.0f;

    public bool isGrounded = false;
    public bool isJumping = false;
    public Rigidbody2D rb;

    private float prevY;

    public void BoostJumpForce()
    {
        jumpForce = jumpForceBoosted;
        CancelInvoke("ResetJumpForce");
        Invoke("ResetJumpForce", jumpForceBstTime);
    }
    public void DecreaseJumpForce()
    {
        jumpForce = jumpDecrease;
        CancelInvoke("ResetJumpForce");
        Invoke("ResetJumpForce", jumpForceBstTime);
    }
    private void ResetJumpForce()
    {
        jumpForce = jumpForceDefault;
    }
    public void BoostSpeed()
    {
        speed = speedBoost;
        CancelInvoke("ResetSpeedForce");
        Invoke("ResetSpeedForce", speedBstTime);
    }
    public void DecreaseSpeed()
    {
        speed = speedDecrease;
        CancelInvoke("ResetSpeedForce");
        Invoke("ResetSpeedForce", speedBstTime);
    }
    private void ResetSpeedForce()
    {
        speed = speedDefault;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0.0f);
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, 0, z);
        transform.Translate(movement * speed * Time.deltaTime);

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                isGrounded = false;
            }
        }
        else
        {
            if (rb.velocity.y == 0 && prevY < 0)
            {
                isGrounded = true;
            }

            prevY = rb.velocity.y;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
        else if (other.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = (ballRb.transform.position - transform.position).normalized;
            ballRb.AddForce(direction, ForceMode2D.Impulse); // delete * jumpForce
            //Debug.Log("Hit");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
