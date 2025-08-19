using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Only one music player allowed
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }
}
