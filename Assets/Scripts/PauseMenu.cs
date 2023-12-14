using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseCanvas;

    private bool isPaused = false;

    void Start()
    {
        pauseCanvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseCanvas.enabled = true;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseCanvas.enabled = false;
        isPaused = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
