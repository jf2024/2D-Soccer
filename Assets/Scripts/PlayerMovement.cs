using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerType { Left, Right }
    public PlayerType playerType;

    public float speed = 5.0f;
    public float jumpForce = 50.0f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float prevY;

    private float jumpForceDefault = 50.0f;
    private float jumpForceBoosted = 70.0f;
    private float jumpForceBoostDuration = 3.5f;

    private float speedDefault = 5.0f;
    private float speedBoost = 7.0f;
    private float speedBoostDuration = 3.5f;

    private float speedDecrease = 3.5f;
    private float jumpDecrease = 40.0f;

    private LayerMask playerLayer; // The layer that represents other players.

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set the appropriate layer mask based on the player's type.
        playerLayer = (playerType == PlayerType.Right) ? LayerMask.NameToLayer("Player1") : LayerMask.NameToLayer("Player2");
    }

    private void Update()
    {
        float moveHorizontal = (playerType == PlayerType.Right) ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal2");
        float x = Input.GetAxis((playerType == PlayerType.Right) ? "Horizontal" : "Horizontal2");
        float z = Input.GetAxis((playerType == PlayerType.Right) ? "Vertical" : "Vertical2");
        Vector3 movement = new Vector3(x, 0, z);
        transform.Translate(speed * Time.deltaTime * movement);

        if (isGrounded)
        {
            string jumpButton = (playerType == PlayerType.Right) ? "Jump" : "Jump2";

            if (Input.GetButtonDown(jumpButton) && !isJumping)
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

        // Check if the player can jump off another player.
        if (CanJumpOffPlayer())
        {
            // Assuming you want to add an extra force when jumping off another player:
            float extraJumpForce = 20.0f; // You can adjust this value.

            // Apply the extra jump force upward.
            rb.AddForce(Vector2.up * extraJumpForce, ForceMode2D.Impulse);
        }
    }
    private bool CanJumpOffPlayer()
    {
        // Cast a ray downward to check if there's another player's collider beneath.
        Vector2 rayOrigin = transform.position;
        rayOrigin.y -= GetComponent<CircleCollider2D>().radius; // Adjust the ray origin to the bottom of the collider.
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.1f, 1 << playerLayer);

        // If the ray hits another player's collider, allow jumping off.
        return hit.collider != null;
    }

    public void BoostJumpForce()
    {
        jumpForce = jumpForceBoosted;
        CancelInvoke("ResetJumpForce");
        Invoke("ResetJumpForce", jumpForceBoostDuration);
    }

    public void DecreaseJumpForce()
    {
        jumpForce = jumpDecrease;
        CancelInvoke("ResetJumpForce");
        Invoke("ResetJumpForce", jumpForceBoostDuration);
    }

    private void ResetJumpForce()
    {
        jumpForce = jumpForceDefault;
    }

    public void BoostSpeed()
    {
        speed = speedBoost;
        CancelInvoke("ResetSpeedForce");
        Invoke("ResetSpeedForce", speedBoostDuration);
    }

    public void DecreaseSpeed()
    {
        speed = speedDecrease;
        CancelInvoke("ResetSpeedForce");
        Invoke("ResetSpeedForce", speedBoostDuration);
    }

    private void ResetSpeedForce()
    {
        speed = speedDefault;
    }

    private void OnCollisionEnter2D(Collision2D other)
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
            ballRb.AddForce(direction, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
