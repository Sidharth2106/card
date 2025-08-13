using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;
    private SpriteRenderer sr;
    public bool isMatched = false;
    private bool isFlipped = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = backSprite; // Start showing back
    }

    public void Flip()
    {
        if (isFlipped || isMatched) return;
        sr.sprite = frontSprite;
        isFlipped = true;
    }

    public void FlipBack()
    {
        if (!isFlipped || isMatched) return;
        sr.sprite = backSprite;
        isFlipped = false;
    }
}
