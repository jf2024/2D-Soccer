using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComputer : Player
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float kickDistanceThreshold = 3.5f; // Adjust the threshold as needed

    protected override void Update()
    {
        if (ShouldKick())
        {
            HandleKicking();
        }
        else if (ShouldJump())
        {
            if (ShouldKick())
            {
                HandleKicking();
            }
            else
            {
                Jump();
            }
        }

        MoveTowardsBall();
    }

    protected override void HandleKicking()
    {
        if (!isKicking)
        {
            isKicking = true;
            kickTimer = 0f;
        }

        if (isKicking)
        {
            kickFoot.eulerAngles += new Vector3(0f, 0f, Time.deltaTime * kickConstant * -1f);
            kickTimer += Time.deltaTime;

            if (kickTimer >= kickDuration)
            {
                isKicking = false;
                StartCoroutine(ReturnToOriginalPosition());
            }
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        while (kickFoot.rotation != startingRot)
        {
            kickFoot.rotation = Quaternion.RotateTowards(kickFoot.rotation, startingRot, Time.deltaTime * kickConstant);
            yield return null;
        }
        kickTimer = 0f;
    }


    private void MoveTowardsBall()
    {
        Vector2 anticipatedBallPosition = PredictFuturePosition(ballTransform.position, ballTransform.GetComponent<Rigidbody2D>().velocity);
        Vector2 dirToAnticipatedBall = (anticipatedBallPosition - (Vector2)transform.position).normalized;

        dirToAnticipatedBall *= base._speed * 0.30f;  // adjust this, try range between 0.20 - 0.30

        base.movement = dirToAnticipatedBall;
        base.Move();
    }

    private Vector2 PredictFuturePosition(Vector2 currentPosition, Vector2 currentVelocity)
    {
        float predictionTime = 1.5f;   //adjust this, numbers between 0.95 - 1

        return currentPosition + currentVelocity * predictionTime;
    }


    private bool ShouldKick()
    {
        Vector2 directionToBall = ballTransform.position - transform.position;
        float distanceToBall = directionToBall.magnitude;

        return (isGrounded || isJumping) && distanceToBall < kickDistanceThreshold;
    }


    private bool ShouldJump()
    {
        Vector2 directionToBall = ballTransform.position - transform.position;
        bool shouldJump = isGrounded && directionToBall.y > 0;

        return shouldJump;
    }


    protected override void Jump()
    {
        if (isGrounded && ShouldJump() && !isJumping)
        {
            base._rigidbody.velocity = new Vector2(base._rigidbody.velocity.x, 0f);
            base._rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            base.isJumping = true;

            StartCoroutine(ResetJumpFlagAfterDelay());
        }
    }

    private IEnumerator ResetJumpFlagAfterDelay()
    {
        yield return new WaitForSeconds(0.2f); // Adjust the delay as needed
        isJumping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (isKicking && collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 kickDirection = (collision.transform.position - kickFoot.position).normalized;

            float verticalDistance = Mathf.Abs(collision.transform.position.y - kickFoot.position.y);
            if (verticalDistance < kickDistance)
            {
                ballRigidbody.velocity = kickDirection * kickForce;

                ballRigidbody.velocity *= 0.5f; // Adjust the factor as needed
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            isJumping = false;
        }
    }
}

/*using System.Collections;
using UnityEngine;

public class PlayerComputer : Player
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float kickDistanceThreshold = 3.5f; // Adjust the threshold as needed
    private bool isPowerupActive = false; // Flag to track whether a power-up is active
    [SerializeField] private float decisionDelay = 0.5f;

    protected override void Update()
    {
        StartCoroutine(MakeDecision());
        MoveTowardsBall();
    }

    private IEnumerator MakeDecision()
    {
        yield return new WaitForSeconds(decisionDelay);

        if (IsBallClose())
        {
            if (isKicking)
            {
                // Continue kicking until the kick animation is complete
                yield break;
            }
            else
            {
                if (isJumping)
                {
                    // Continue jumping until the jump animation is complete
                    yield break;
                }
                else
                {
                    if (ShouldKick())
                    {
                        // Initiate kick
                        InitiateKick();
                    }
                    else
                    {
                        if (ShouldJump())
                        {
                            // Initiate jump
                            InitiateJump();
                        }
                        else
                        {
                            // Stay idle or perform other actions
                            yield break;
                        }
                    }
                }
            }
        }
        else
        {
            // Ball is not close
            if (ShouldKick())
            {
                // Initiate kick
                InitiateKick();
            }
            else
            {
                if (ShouldJump())
                {
                    // Initiate jump
                    InitiateJump();
                }
                else
                {
                    // Stay idle or perform other actions
                    yield break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Powerup") && !isPowerupActive)
        {
            Powerup powerup = other.GetComponent<Powerup>();
            ApplyPowerupEffects(powerup);

            isPowerupActive = true;
            StartCoroutine(ResetPowerupFlag(powerup.duration));
        }
    }

    private void ApplyPowerupEffects(Powerup powerup)
    {
        switch (powerup.powerupType)
        {
            case Powerup.PowerupType.SpeedUp:
                BoostSpeed(powerup.boostValue, powerup.duration);
                break;
            case Powerup.PowerupType.SpeedLess:
                DecreaseSpeed(powerup.boostValue, powerup.duration);
                break;
            case Powerup.PowerupType.JumpBoost:
                BoostJumpForce(powerup.boostValue, powerup.duration);
                break;
            case Powerup.PowerupType.JumpLess:
                DecreaseJumpForce(powerup.boostValue, powerup.duration);
                break;
        }
    }
    private bool IsBallClose()
    {
        Vector2 directionToBall = ballTransform.position - transform.position;
        float distanceToBall = directionToBall.magnitude;
        return distanceToBall < kickDistanceThreshold;
    }

    private void MoveTowardsBall()
    {
        Vector2 anticipatedBallPosition = PredictFuturePosition(ballTransform.position, ballTransform.GetComponent<Rigidbody2D>().velocity);
        Vector2 dirToAnticipatedBall = (anticipatedBallPosition - (Vector2)transform.position).normalized;

        dirToAnticipatedBall *= base._speed * 0.21f;  // adjust this, try range between 0.20 - 0.30

        base.movement = dirToAnticipatedBall;
        base.Move();
    }

    private Vector2 PredictFuturePosition(Vector2 currentPosition, Vector2 currentVelocity)
    {
        float predictionTime = 1f; 

        return currentPosition + currentVelocity * predictionTime;
    }

    private bool ShouldKick()
    {
        Vector2 directionToBall = ballTransform.position - transform.position;
        float distanceToBall = directionToBall.magnitude;

        return (isGrounded || isJumping) && distanceToBall < kickDistanceThreshold;
    }

    private bool ShouldJump()
    {
        Vector2 directionToBall = ballTransform.position - transform.position;
        bool shouldJump = isGrounded && directionToBall.y > 0;

        return shouldJump;
    }

    private void InitiateKick()
    {
        // Example: Trigger kick animation and perform kick logic
        if (!isKicking)
        {
            isKicking = true;
            kickTimer = 0f;
            StartCoroutine(ReturnToOriginalPosition());
        }
    }

    private void InitiateJump()
    {
        // Example: Trigger jump animation and perform jump logic
        if (isGrounded && !isJumping)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            StartCoroutine(ResetJumpFlagAfterDelay());
        }
    }

    private IEnumerator ResetJumpFlagAfterDelay()
    {
        yield return new WaitForSeconds(0.2f); // Adjust the delay as needed
        isJumping = false;
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        while (kickFoot.rotation != startingRot)
        {
            kickFoot.rotation = Quaternion.RotateTowards(kickFoot.rotation, startingRot, Time.deltaTime * kickConstant);
            yield return null;
        }
        kickTimer = 0f;
    }

    private IEnumerator ResetPowerupFlag(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPowerupActive = false;
    }
}*/

