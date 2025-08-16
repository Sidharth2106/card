using UnityEngine;
using TMPro;

public class WinPanelUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text clickText;

    void OnEnable()
    {
        if (MemoryGameManager.instance != null)
        {
            scoreText.text = "Final Score: " + MemoryGameManager.instance.score;
            clickText.text = "Total Clicks: " + MemoryGameManager.instance.clicks;
        }
    }
}
