using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource; // <--- NEW: For background music

    [Header("SFX Clips")]
    public AudioClip clickSound;
    public AudioClip matchSound;
    public AudioClip wrongSound;
    public AudioClip winSound;

    [Header("Music")]
    public AudioClip backgroundMusic; // <--- NEW: Assign your BGM clip here

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up music source if assigned and clip exists
            if (musicSource != null && backgroundMusic != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Optional: Call this if you ever want to change music at runtime
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Optional: Call to mute/unmute music
    public void SetMusicMuted(bool isMuted)
    {
        if (musicSource != null)
            musicSource.mute = isMuted;
    }

    // Optional: Call to mute/unmute SFX
    public void SetSFXMuted(bool isMuted)
    {
        if (sfxSource != null)
            sfxSource.mute = isMuted;
    }
}
