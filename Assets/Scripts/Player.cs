using System.Collections;
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

    private float originalSpeed; 
    private float originalJumpForce; 

    public Vector2 movement;

    protected Rigidbody2D _rigidbody;
    protected PlayerInput _playerInput;
    [SerializeField] protected bool isGrounded = false;
    protected bool isJumping = false; 

    public Transform kickFoot;
    public bool isKicking = false;
    public float kickTimer = 0.0f;
    public Quaternion startingRot;

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

        originalSpeed = _speed;
        originalJumpForce = _jumpForce;
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
            if (_playerInput.actions[$"{playerID}Jump"].ReadValue<float>() > 0)
            {
                _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    protected virtual void HandleKicking()
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
            float verticalDistance = Mathf.Abs(collision.transform.position.y - kickFoot.position.y);

            Vector2 kickDirection = (collision.transform.position - kickFoot.position).normalized;

            if (verticalDistance < kickDistance)
            {
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

    public void BoostSpeed(float boostValue, float duration)
    {
        _speed += boostValue;
        StartCoroutine(ResetPowerup(() => _speed = originalSpeed, duration));
    }

    public void DecreaseSpeed(float boostValue, float duration)
    {
        // Multiply by a factor to decrease speed
        float speedFactor = 0.5f; // You can adjust this factor
        _speed -= boostValue * speedFactor;

        StartCoroutine(ResetPowerup(() =>
        {
            _speed = originalSpeed;
            movement.x *= -1;
        }, duration));
    }


    public void BoostJumpForce(float boostValue, float duration)
    {
        _jumpForce += boostValue;
        StartCoroutine(ResetPowerup(() => _jumpForce = originalJumpForce, duration));
    }

    public void DecreaseJumpForce(float boostValue, float duration)
    {
        _jumpForce -= boostValue;
        StartCoroutine(ResetPowerup(() => _jumpForce = originalJumpForce, duration));
    }

    private IEnumerator ResetPowerup(System.Action resetAction, float duration)
    {
        yield return new WaitForSeconds(duration);
        resetAction.Invoke();
    }

    public void ResetPowerUpEffects()
    {
        _speed = originalSpeed;
        _jumpForce = originalJumpForce;
    }

}
