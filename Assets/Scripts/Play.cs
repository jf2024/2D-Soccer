using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Game");
        Time.timeScale = 1;
    }
}
