using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerupScreen : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("PowerUps");
        //Time.timeScale = 1;
    }
}
