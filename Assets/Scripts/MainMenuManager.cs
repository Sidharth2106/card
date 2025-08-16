using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Replace "GameScene" with your actual game scene name
        SceneManager.LoadScene("PreLevel1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
