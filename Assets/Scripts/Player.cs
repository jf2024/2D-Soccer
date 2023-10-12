using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerID
{
    Player1,
    Player2
}

public class Player : MonoBehaviour
{
    [SerializeField] protected float _speed = 5.0f;
    [SerializeField] protected float _jumpForce = 23.0f;

    private Vector2 movement;

    protected Rigidbody2D _rigidbody;
    protected PlayerInput _playerInput;
    [SerializeField] protected bool isGrounded = false;

    public Transform kickFoot;
    private bool isKicking = false;
    private float kickTimer = 0.0f;
    private Quaternion startingRot;

    public float kickForce = 18.0f;
    public float kickDistance = 1.3f;
    public float kickDuration = 0.50f;
    public float kickConstant = 750.0f;

    public PlayerID playerID; 

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }
    protected virtual void Start()
    {
        Time.timeScale = 1;
        startingRot = kickFoot.rotation;
    }
    protected virtual void FixedUpdate()
    {
        Jump();
        Move();
    }
    protected virtual void Update()
    {
        HandleKicking();
    }

    protected virtual void Move()
    {
        _rigidbody.velocity = new Vector2(movement.x * _speed, _rigidbody.velocity.y);
    }

    protected virtual void Jump()
    {
        if (isGrounded)
        {
            // Check if jump action is currently held down
            if (_playerInput.actions[$"{playerID}Jump"].ReadValue<float>() > 0)
            {
                _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    private void HandleKicking()
    {
        if (_playerInput.actions[$"{playerID}Kick"].triggered && !isKicking)
        {
            isKicking = true;
            kickTimer = 0.0f;
        }

        if (isKicking)
        {
            float rotationDirection = (playerID == PlayerID.Player1) ? 1f : -1f;
            kickFoot.eulerAngles += new Vector3(0f, 0f, Time.deltaTime * kickConstant * rotationDirection);
            kickTimer += Time.deltaTime;

            if (kickTimer >= kickDuration)
            {
                isKicking = false;
                kickFoot.rotation = startingRot;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (isKicking && collision.gameObject.CompareTag("Ball"))
        {
            // Vertical distance between the foot and the ball
            float verticalDistance = Mathf.Abs(collision.transform.position.y - kickFoot.position.y);

            // Calculate the direction from the player to the ball
            Vector2 kickDirection = (collision.transform.position - kickFoot.position).normalized;

            // Check if the ball is close enough to the foot
            if (verticalDistance < kickDistance)
            {
                // Apply force to the ball based on the player's facing direction
                Vector2 kickForceStrength = (collision.transform.position - kickFoot.position).normalized * kickForce;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(kickForceStrength, ForceMode2D.Impulse);
            }
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
