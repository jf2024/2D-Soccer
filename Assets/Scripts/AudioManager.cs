using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource startWhistleSource;
    [SerializeField] private AudioSource endWhistleSource;
    [SerializeField] private AudioSource backgroundNoiseSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayStartWhistle()
    {
        if (startWhistleSource != null)
        {
            startWhistleSource.Play();
        }
    }

    public void PlayEndWhistle()
    {
        if (endWhistleSource != null)
        {
            endWhistleSource.Play();
        }
    }

    public void PlayBackgroundNoise()
    {
        if (backgroundNoiseSource != null && !backgroundNoiseSource.isPlaying)
        {
            backgroundNoiseSource.Play();
        }
    }

    public void StopBackgroundNoise()
    {
        if (backgroundNoiseSource != null)
        {
            backgroundNoiseSource.Stop();
        }
    }
}
