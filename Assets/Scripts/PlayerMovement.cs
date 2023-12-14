using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputAction movementAction = (playerType == PlayerType.Right) ? playerInput.actions["Player2Movement"] : playerInput.actions["Player1Movement"];

        float moveInput = movementAction.ReadValue<float>();

        Vector3 movement = new Vector3(moveInput, 0, 0);
        transform.Translate(speed * Time.deltaTime * movement);

        if (isGrounded)
        {
            InputAction jumpAction = (playerType == PlayerType.Right) ? playerInput.actions["Player2Jump"] : playerInput.actions["Player1Jump"];
            if (jumpAction.ReadValue<float>() > 0 && !isJumping)
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




