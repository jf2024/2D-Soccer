using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CREATE ENUM FOR THIS 

public class JumpPowerup : MonoBehaviour
{
    public float jumpBoost = 70f; // how much to increase the player's jump
    public float jumpDuration = 5f; // how long the player's jump is increased

    private bool isJumpBoosted = false;
    private float jumpBoostEndTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // check if the collider belongs to a player
        {
            RightPlayerMovement rightPlayerMovement = other.GetComponent<RightPlayerMovement>();
            if (rightPlayerMovement != null && other.gameObject.name == "Player2") // check if the player is the right player
            {
                if (!isJumpBoosted)
                {
                    isJumpBoosted = true;
                    jumpBoostEndTime = Time.time + jumpDuration; // set the end time for the jump boost
                    rightPlayerMovement.BoostJumpForce();
                }
                Powerups.powerupsCount--;
                Destroy(gameObject); // destroy the power-up
            }

            LeftPlayerMovement leftPlayerMovement = other.GetComponent<LeftPlayerMovement>();
            if (leftPlayerMovement != null && other.gameObject.name == "Player1") // check if the player is the left player
            {
                if (!isJumpBoosted)
                {
                    isJumpBoosted = true;
                    jumpBoostEndTime = Time.time + jumpDuration; // set the end time for the jump boost
                    leftPlayerMovement.BoostJumpForce(); //NEW STUFF HERE
                }
                Powerups.powerupsCount--;
                Destroy(gameObject); // destroy the power-up
            }
        }
    }

}
