using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MemoryGameManager : MonoBehaviour
{
    [System.Serializable]
    public class CardPair
    {
        public GameObject card1;
        public GameObject card2;
    }

    public List<CardPair> correctPairs;
    public float flipBackDelay = 1f;

    private GameObject firstSelected = null;
    private GameObject secondSelected = null;
    private bool canClick = true;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text clickText;
    public GameObject winPanel;

    [Header("Scoring Settings")]
    public int score = 0;
    public int clicks = 0;

    [Header("Pop Animation Settings")]
    public float popScale = 1.2f;
    public float popDuration = 0.2f;

    void Start()
    {
        UpdateScore(0);
        UpdateClicks(0);
        if (winPanel != null)
            winPanel.SetActive(false); // hide at start
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

        // Play card click sound from SoundManager
        SoundManager.instance.PlaySFX(SoundManager.instance.clickSound);

        cardScript.Flip();
        UpdateClicks(1);

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

            UpdateScore(5);
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

        // Play win sound
        SoundManager.instance.PlaySFX(SoundManager.instance.winSound);

        // Small delay before showing win panel (sync with sound)
        if (SoundManager.instance.winSound != null)
            yield return new WaitForSeconds(SoundManager.instance.winSound.length * 0.8f);

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
        StartCoroutine(PopText(scoreText));
    }

    public void UpdateClicks(int amount)
    {
        clicks += amount;
        clickText.text = "Clicks: " + clicks;
        StartCoroutine(PopText(clickText));
    }

    private IEnumerator PopText(TMP_Text textObj)
    {
        Vector3 originalScale = textObj.transform.localScale;
        Vector3 targetScale = originalScale * popScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (popDuration / 2);
            textObj.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (popDuration / 2);
            textObj.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
    }
}