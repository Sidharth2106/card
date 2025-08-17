using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;  // Pause game time
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;  // Resume game time
        isPaused = false;
    }

    public void BackToLevelSelect()
    {
        Time.timeScale = 1f;  // Ensure timeScale reset when leaving
        SceneManager.LoadScene("LevelSelect");  // Load level select scene
    }
}
