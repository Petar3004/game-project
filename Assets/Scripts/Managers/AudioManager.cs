using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("--- Audio Clip ---")]
    public AudioClip backgroundChapter1;
    public AudioClip backgroundChapter2;
    public AudioClip backgroundChapter3;
    public AudioClip death;
    public AudioClip levelComplete;
    public AudioClip roomComplete;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic(int levelIndex)
    {
        if (levelIndex <= 3)
        {
            musicSource.clip = backgroundChapter1;
        }
        else if (levelIndex <= 6)
        {
            musicSource.clip = backgroundChapter2;
        }
        else
        {
            musicSource.clip = backgroundChapter3;
        }
        musicSource.Play();
    }
}
