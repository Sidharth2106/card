using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameManager : MonoBehaviour
{
    [System.Serializable]
    public class CardPair
    {
        public GameObject card1;
        public GameObject card2;
    }

    public List<CardPair> correctPairs; // Set pairs in Inspector
    public float flipBackDelay = 1f;

    private GameObject firstSelected = null;
    private GameObject secondSelected = null;
    private bool canClick = true;

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

        cardScript.Flip();

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
        yield return new WaitForSeconds(0.5f); // Wait for flip animation

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
        }
        else
        {
            yield return new WaitForSeconds(flipBackDelay);
            firstSelected.GetComponent<Card>().FlipBack();
            secondSelected.GetComponent<Card>().FlipBack();
        }

        firstSelected = null;
        secondSelected = null;
        canClick = true;

        // Check if all pairs are found
        if (AllPairsFound())
        {
            Debug.Log("All pairs found! Scene complete!");
            // Optional: Load next scene or show Win UI
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
}

