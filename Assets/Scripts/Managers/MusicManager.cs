using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Level Music")]
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;

    private AudioClip currentClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip clip = GetMusicForLevel(scene.buildIndex);
        if (clip != null)
        {
            PlayMusic(clip);
        }
    }

    AudioClip GetMusicForLevel(int levelIndex)
    {
        return levelIndex switch
        {
            1 => level1Music,
            2 => level2Music,
            3 => level3Music,
            _ => null
        };
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == currentClip) return;

        StopAllCoroutines();
        StartCoroutine(FadeAndSwitch(clip));
    }

    IEnumerator FadeAndSwitch(AudioClip newClip)
    {
        if (musicSource.isPlaying)
        {
            yield return Fade(1f, 0f);
        }

        currentClip = newClip;
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        yield return Fade(0f, 1f);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = to;
    }
}
