using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Winner : MonoBehaviour
{
    public TextMeshProUGUI p1;
    public TextMeshProUGUI p2;
    public TextMeshProUGUI winner;

    private int score1;
    private int score2;

    private void Update()
    {
        score1 = p1.text[4];
        score2 = p2.text[4];

        // Check scores and print winner 
        if (score1 > score2)
        {
            winner.text = "P1 wins!";
            GameManager.Instance.p1stats.wins++;
            GameManager.Instance.p2stats.loss++;
        }
        else if (score2 > score1)
        {
            winner.text = "P2 wins!";
            GameManager.Instance.p2stats.wins++;
            GameManager.Instance.p1stats.loss++;
        }
        else
        {
            winner.text = "Draw!";
            GameManager.Instance.p1stats.draw++;
            GameManager.Instance.p2stats.draw++;
        }

        GameManager.Instance.getPlayerStats(1);
    }
}