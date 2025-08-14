using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PreLevelManager : MonoBehaviour
{
    public float previewTime = 2f; // seconds
    public string nextSceneName = "Level1";

    void Start()
    {
        StartCoroutine(StartLevelAfterPreview());
    }

    IEnumerator StartLevelAfterPreview()
    {
        // Optional: Add animation to cards here

        yield return new WaitForSeconds(previewTime);

        // Load actual level scene
        SceneManager.LoadScene(nextSceneName);
    }
}

