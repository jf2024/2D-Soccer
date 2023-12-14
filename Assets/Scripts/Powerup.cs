using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        SpeedUp,
        SpeedLess,
        JumpBoost,
        JumpLess
    }

    public PowerupType powerupType;
    public float boostValue = 7f;
    public float duration = 5f;

    private bool isBoosted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                if (!isBoosted)
                {
                    isBoosted = true;

                    switch (powerupType)
                    {
                        case PowerupType.SpeedUp:
                            player.BoostSpeed(boostValue, duration);
                            break;
                        case PowerupType.SpeedLess:
                            player.DecreaseSpeed(boostValue, duration);
                            break;
                        case PowerupType.JumpBoost:
                            player.BoostJumpForce(boostValue, duration);
                            break;
                        case PowerupType.JumpLess:
                            player.DecreaseJumpForce(boostValue, duration);
                            break;
                    }

                    StartCoroutine(ResetPowerup());
                }

                Powerups.powerupsCount--;
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ResetPowerup()
    {
        yield return new WaitForSeconds(duration);
        isBoosted = false;
    }
}
