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
            if (ShouldKick())  // Check if the bot should kick while jumping
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
                StartCoroutine(ReturnToOriginalPosition()); // Start the coroutine to return to the original position
            }
        }
    }

    // Coroutine to return the foot to its original position
    private IEnumerator ReturnToOriginalPosition()
    {
        while (kickFoot.rotation != startingRot)
        {
            kickFoot.rotation = Quaternion.RotateTowards(kickFoot.rotation, startingRot, Time.deltaTime * kickConstant);
            yield return null;
        }

        // Reset the kickTimer
        kickTimer = 0f;
    }


    private void MoveTowardsBall()
    {
        // Calculate the direction from the bot to the anticipated future position of the ball
        Vector2 anticipatedBallPosition = PredictFuturePosition(ballTransform.position, ballTransform.GetComponent<Rigidbody2D>().velocity);
        Vector2 dirToAnticipatedBall = (anticipatedBallPosition - (Vector2)transform.position).normalized;

        dirToAnticipatedBall *= base._speed * 0.21f;  // adjust this, try range between 0.20 - 0.30

        // Set the movement vector
        base.movement = dirToAnticipatedBall;
        base.Move();
    }

    private Vector2 PredictFuturePosition(Vector2 currentPosition, Vector2 currentVelocity)
    {
        float predictionTime = 1f;   //adjust this, numbers between 0.95 - 1

        return currentPosition + currentVelocity * predictionTime;
    }


    private bool ShouldKick()
    {
        // Check if the ball is near the bot based on distance
        Vector2 directionToBall = ballTransform.position - transform.position;
        float distanceToBall = directionToBall.magnitude;

        // Return true if the bot is grounded or jumping and the ball is close
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
            base._rigidbody.velocity = new Vector2(base._rigidbody.velocity.x, 0f); // Reset vertical velocity before each jump
            base._rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            base.isJumping = true;

            // Optionally, reset isJumping after a delay to prevent immediate consecutive jumps
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
            // Get the Rigidbody2D component of the ball
            Rigidbody2D ballRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // Calculate the direction from the bot to the ball
            Vector2 kickDirection = (collision.transform.position - kickFoot.position).normalized;

            // Check if the ball is close enough to the foot
            float verticalDistance = Mathf.Abs(collision.transform.position.y - kickFoot.position.y);
            if (verticalDistance < kickDistance)
            {
                // Set the velocity of the ball based on the player's facing direction and kick force
                ballRigidbody.velocity = kickDirection * kickForce;

                // Optionally, you can dampen the ball's velocity if needed
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




