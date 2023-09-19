using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerupScreen : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Powerups");
        //Time.timeScale = 1;
    }
}
