using UnityEngine;
using UnityEngine.UI;

public class SoundToggleUI : MonoBehaviour
{
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image iconImage; // Reference to the Image component on your button

    private bool isMuted = false;

    public void ToggleSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateIcon();
    }

    void Start()
    {
        // Load sound preference if you store it
        UpdateIcon();
    }

    void UpdateIcon()
    {
        iconImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
