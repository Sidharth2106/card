using UnityEngine;

public class HelpPanelToggle : MonoBehaviour
{
    public GameObject helpPanel;  // Assign your Help Panel GameObject here

    public void ShowHelp()
    {
        if (helpPanel != null)
            helpPanel.SetActive(true);
    }

    public void HideHelp()
    {
        if (helpPanel != null)
            helpPanel.SetActive(false);
    }
}
