using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist AudioManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlayLoopingSound(AudioClip clip)
    {
        // Implement audio looping logic here
    }

    public void StopLoopingSound(AudioClip clip)
    {
        // Implement audio stopping logic here
    }

    // Add more audio-related methods as needed
}
