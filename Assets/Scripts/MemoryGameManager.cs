using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryGameManager : MonoBehaviour
{
    [System.Serializable]
    public class CardPair
    {
        public GameObject card1;
        public GameObject card2;
    }

    public static MemoryGameManager instance;  // Singleton reference

    public List<CardPair> correctPairs;
    public float flipBackDelay = 1f;

    private GameObject firstSelected = null;
    private GameObject secondSelected = null;
    private bool canClick = true;

    [Header("UI References")]
    public GameObject winPanel;

    [Header("Scoring Settings")]
    public int score = 0;
    public int clicks = 0;

    void Awake()
    {
        instance = this; // Register global access
    }

    void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false); // Hide WinPanel at start
    }

    void Update()
    {
        if (!canClick) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Card"))
            {
                OnCardClicked(hit.collider.gameObject);
            }
        }
    }

    void OnCardClicked(GameObject clickedCard)
    {
        Card cardScript = clickedCard.GetComponent<Card>();
        if (firstSelected == clickedCard || cardScript.isMatched)
            return;

        // Play card click sound
        SoundManager.instance.PlaySFX(SoundManager.instance.clickSound);

        cardScript.Flip();
        clicks++; // Count total attempts

        if (firstSelected == null)
        {
            firstSelected = clickedCard;
        }
        else
        {
            secondSelected = clickedCard;
            canClick = false;
            StartCoroutine(CheckPair());
        }
    }

    IEnumerator CheckPair()
    {
        yield return new WaitForSeconds(0.5f);

        bool isMatch = false;
        foreach (CardPair pair in correctPairs)
        {
            if ((firstSelected == pair.card1 && secondSelected == pair.card2) ||
                (firstSelected == pair.card2 && secondSelected == pair.card1))
            {
                isMatch = true;
                break;
            }
        }

        if (isMatch)
        {
            firstSelected.GetComponent<Card>().isMatched = true;
            secondSelected.GetComponent<Card>().isMatched = true;

            // Play match sound
            SoundManager.instance.PlaySFX(SoundManager.instance.matchSound);

            score += 5; // Add score for match
        }
        else
        {
            // Play wrong/mismatch sound
            SoundManager.instance.PlaySFX(SoundManager.instance.wrongSound);

            yield return new WaitForSeconds(flipBackDelay);
            firstSelected.GetComponent<Card>().FlipBack();
            secondSelected.GetComponent<Card>().FlipBack();
        }

        firstSelected = null;
        secondSelected = null;
        canClick = true;

        if (AllPairsFound())
        {
            canClick = false;
            StartCoroutine(ShowWinPanelWithDelay(0.5f));
        }
    }

    bool AllPairsFound()
    {
        foreach (CardPair pair in correctPairs)
        {
            if (!pair.card1.GetComponent<Card>().isMatched || !pair.card2.GetComponent<Card>().isMatched)
                return false;
        }
        return true;
    }

    IEnumerator ShowWinPanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ✅ Unlock next level immediately after win
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("levelUnlocked", 1);

        // Scene 0 = LevelSelect, so Level 1 starts at build index 1
        if (unlockedLevel < currentIndex + 1)
        {
            PlayerPrefs.SetInt("levelUnlocked", currentIndex + 1);
            PlayerPrefs.Save();
            Debug.Log("Unlocked level progress updated to: " + (currentIndex + 1));
        }

        // Play win sound
        SoundManager.instance.PlaySFX(SoundManager.instance.winSound);

        if (SoundManager.instance.winSound != null)
            yield return new WaitForSeconds(SoundManager.instance.winSound.length * 0.5f);

        if (winPanel != null)
            winPanel.SetActive(true); // Show WinPanel
    }

    // ✅ Called by Next Level button on WinPanel
    public void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        // Ensure progress is saved (just in case)
        int unlockedLevel = PlayerPrefs.GetInt("levelUnlocked", 1);
        if (unlockedLevel < currentIndex + 1)
        {
            PlayerPrefs.SetInt("levelUnlocked", currentIndex + 1);
            PlayerPrefs.Save();
        }

        // After unlocking → go back to LevelSelect menu
        SceneManager.LoadScene("LevelSelect");
    }
}
