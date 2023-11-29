using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComputer : Player
{
    [SerializeField] private Transform ballTransform;
    private bool isJumping = false;

    protected override void Update()
    {
        AIBehavior();
    }

    private void AIBehavior()
    {
        float ballDistance = Vector2.Distance(transform.position, ballTransform.position);

        if (ballDistance < kickDistance)
        {
            if (ShouldKick())
            {
                HandleKicking();
            }
            else if (!isJumping && ShouldJump())
            {
                Jump();
            }
        }
        else
        {
            isJumping = false; // Reset jump state when not close to the ball
            MoveTowardsBall(); // Move towards the ball
        }
    }

    private bool ShouldKick()
    {
        // Implement kick decision logic based on factors like ball position and player's state
        // For example, kick if the ball is at a favorable position and the player is not currently kicking.
        return !isKicking && Random.value < 0.5f; // Adjust the probability as needed
    }

    private bool ShouldJump()
    {
        // Implement jump decision logic based on factors like ball position and player's state
        // For example, jump if the ball is above the player and the player is not currently jumping.
        return isGrounded && Random.value < 0.2f;
    }

    private void MoveTowardsBall()
    {
        // Implement movement logic to get closer to the ball
        // For example, move towards the ball with a random chance.
        if (Random.value < 0.5f)
        {
            Move(); // Move right
        }
        else
        {
            Move(); // Move left
        }
    }

    protected override void HandleKicking()
    {
        isKicking = true;

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

    protected override void Jump()
    {
        if (isGrounded)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            isJumping = true; // Set the jump state
        }
    }
}
