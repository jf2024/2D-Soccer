using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalsController : MonoBehaviour
{
    [SerializeField]
    private int playerID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.GoalScored(playerID);
        }
    }
}
