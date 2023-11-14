using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoPlayerGame : MonoBehaviour
{

    public void LoadGameScene()
    {
        SceneManager.LoadScene("TwoPlayerGame");
        Time.timeScale = 1;
    }
}







