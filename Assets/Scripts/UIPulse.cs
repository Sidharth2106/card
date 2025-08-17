using UnityEngine;

public class UIPulse : MonoBehaviour
{
    public float speed = 2f;        // How fast it pulses
    public float minScale = 1f;     // Minimum scale
    public float maxScale = 1.1f;   // Maximum scale

    void Update()
    {
        float t = 0.5f + 0.5f * Mathf.Sin(Time.time * speed);
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
