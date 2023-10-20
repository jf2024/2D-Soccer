using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLeft = 90.0f;
    public TextMeshProUGUI timerText;
    public Canvas gameOverCanvas;

    public AudioSource startWhis;
    public AudioSource endWhis;

    private bool hasPlayedStart = false;
    private bool hasPlayedEnd = false;

    void Start()
    {
        startWhis.PlayOneShot(startWhis.clip, 1f);
    }

    void Update()
    {
        if (!hasPlayedStart)
        {
            startWhis.PlayOneShot(startWhis.clip, 1f);
            hasPlayedStart = true;
        }

        timeLeft -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeLeft / 60.0f);
        int seconds = Mathf.FloorToInt(timeLeft % 60.0f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeLeft <= 0)
        {
            Time.timeScale = 0;

            if (!hasPlayedEnd)
            {
                endWhis.PlayOneShot(endWhis.clip, 1f);
                hasPlayedEnd = true;
            }

            timerText.text = "00:00";
            gameOverCanvas.enabled = true;
        }
    }
}
