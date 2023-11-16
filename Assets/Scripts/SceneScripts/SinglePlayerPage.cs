using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerPage : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("SinglePlayerMenu");
    }
}