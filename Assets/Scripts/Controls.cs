using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Controls");
        Time.timeScale = 1;
    }
}
