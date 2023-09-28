/*using System.Collections;
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

    public LayerMask playerLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

            // Check if the jump button is being held down
            if (Input.GetButton(jumpButton) && !isJumping)
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

        // Check if the player is above another player
        if (IsPlayerAboveAnotherPlayer())
        {
            // Allow jumping even if grounded
            isGrounded = true;
        }
    }
*//*    private void FixedUpdate()
    {
        // Check for overlapping with other players' colliders
        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius, playerLayer);

        // If there are overlapped colliders (other players), allow jumping
        if (overlappedColliders.Length > 1)
        {
            isJumping = false;
        }
    }*//*

    private bool IsPlayerAboveAnotherPlayer()
    {
        // Cast ray downwards from the center of the player's collider
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Player"));

        // Check if the ray hit another player
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            return true;
        }

        return false;
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
*//*        else if (other.gameObject.CompareTag("Player"))
        {
            Vector2 pushDirection = (other.transform.position - transform.position).normalized;

            // Apply a force to push the players away from each other
            float pushForce = 50.0f; // Adjust this value as needed
            rb.AddForce(-pushDirection * pushForce, ForceMode2D.Impulse);
        }*//*
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Import the Input System namespace

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


    private PlayerInput playerInput; // Reference to the PlayerInput component

    // Initialize the PlayerInput component
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
        // Determine which player's input to process based on the playerType
        InputAction movementAction = (playerType == PlayerType.Right) ? playerInput.actions["Player2Movement"] : playerInput.actions["Player1Movement"];

        // Read the movement input value
        float moveInput = movementAction.ReadValue<float>();

        // Apply movement
        Vector3 movement = new Vector3(moveInput, 0, 0);
        transform.Translate(speed * Time.deltaTime * movement);

        if (isGrounded)
        {
            // Determine which player's input to process based on the playerType
            InputAction jumpAction = (playerType == PlayerType.Right) ? playerInput.actions["Player2Jump"] : playerInput.actions["Player1Jump"];

            // Check if the jump button is being held down
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

        // Check if the player is above another player
        if (IsPlayerAboveAnotherPlayer())
        {
            // Allow jumping even if grounded
            isGrounded = true;
        }
    }

    private bool IsPlayerAboveAnotherPlayer()
    {
        // Cast ray downwards from the center of the player's collider
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Player"));

        // Check if the ray hit another player
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            return true;
        }

        return false;
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




