using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public Button[] levelButtons; // Drag buttons in order: 1→5

    void Start()
    {
        // Load saved progress: default = 1 (only Level 1 unlocked)
        int unlockedLevel = PlayerPrefs.GetInt("levelUnlocked", 1);

        Debug.Log("Unlocked Level Progress: " + unlockedLevel);

        // Loop through all buttons
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < unlockedLevel)
                levelButtons[i].interactable = true;   // Available
            else
                levelButtons[i].interactable = false;  // Locked
        }
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
