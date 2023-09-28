using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpForce = 23.0f;  //jump should be between 20-25 range

    private Rigidbody2D _rigidbody;
    private Vector2 movement;

    private bool isGrounded = false;

    private InputAction _jumpAction; // Reference to the jump action

    public enum PlayerType { Left, Right }
    public PlayerType playerType;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        // Get that reference 
        _jumpAction = GetComponent<PlayerInput>().actions["Player1Jump"];
    }

    private void FixedUpdate()
    {
        Jump();
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(movement.x * _speed, _rigidbody.velocity.y);

        if (isGrounded)
        {
            // Check if the jump action is currently held down
            if (_jumpAction.ReadValue<float>() > 0)
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}




