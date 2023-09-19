using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
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
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    public void MainMenu()
    {
        // Load the main menu scene
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

