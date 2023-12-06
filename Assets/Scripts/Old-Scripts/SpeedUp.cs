using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float speedBoost = 7f; 
    public float speedDuration = 5f; 

    private bool isSpeedBoosted = false;
    private float speedBoostEndTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            RightPlayerMovement rightPlayerMovement = other.GetComponent<RightPlayerMovement>();
            if (rightPlayerMovement != null && other.gameObject.name == "Player2") 
            {
                if (!isSpeedBoosted)
                {
                    isSpeedBoosted = true;
                    speedBoostEndTime = Time.time + speedDuration;
                    rightPlayerMovement.BoostSpeed();
                }
                Powerups.powerupsCount--;
                Destroy(gameObject);
            }

            LeftPlayerMovement leftPlayerMovement = other.GetComponent<LeftPlayerMovement>();
            if (leftPlayerMovement != null && other.gameObject.name == "Player1") // check if the player is the left player
            {
                if (!isSpeedBoosted)
                {
                    isSpeedBoosted = true;
                    speedBoostEndTime = Time.time + speedDuration; // set the end time for the jump boost
                    leftPlayerMovement.BoostSpeed(); //NEW STUFF HERE
                }
                Powerups.powerupsCount--;
                Destroy(gameObject); // destroy the power-up
            }
        }
    }

}
