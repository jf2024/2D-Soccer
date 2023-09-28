using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpForce = 23.0f; // Jump force in the 20-25 range, play around with this

    private Rigidbody2D _rigidbody;
    private Vector2 movement;
    private bool isGrounded = false;

    private PlayerInput _playerInput;

    // Reference to foot 
    public Transform kickFoot;

    // Kicking stuff
    private bool isKicking = false;
    private float kickTimer = 0.0f;
    private Quaternion startingRot;

    //play around with all these variables here
    public float kickForce = 18.0f;
    public float kickDistance = 1.3f;
    public float kickDuration = 0.50f;
    public float kickConstant = 750.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Time.timeScale = 1;
        startingRot = kickFoot.rotation;
    }

    private void Update()
    {
        // kicking logic using the new Input System
        if (_playerInput.actions["Player1Kick"].triggered && !isKicking)
        {
            isKicking = true;
            kickTimer = 0.0f;
        }

        if (isKicking)
        {
            // Rotate the kick foot
            kickFoot.eulerAngles += new Vector3(0f, 0f, Time.deltaTime * kickConstant);
            kickTimer += Time.deltaTime;

            if (kickTimer >= kickDuration)
            {
                isKicking = false;
                kickFoot.rotation = startingRot;
            }
        }
    }

    private void FixedUpdate()
    {
        // Handle movement and jumping
        Move();
        Jump();
    }

    private void Move()
    {
        _rigidbody.velocity = new Vector2(movement.x * _speed, _rigidbody.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            // Check if the jump action is currently held down
            if (_playerInput.actions["Player1Jump"].ReadValue<float>() > 0)
            {
                _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnPlayer1Move(InputValue inputValue)
    {
        movement = inputValue.Get<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Check for collision with the ball during a kick
        if (isKicking && collision.gameObject.CompareTag("Ball"))
        {
            // Apply extra force to ball
            Vector2 kickForceStrength = (collision.transform.position - kickFoot.position).normalized * kickForce;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(kickForceStrength, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
