using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayer : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("SinglePlayerGame");
        Time.timeScale = 1;
    }
}