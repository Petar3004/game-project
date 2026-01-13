using UnityEngine;

public enum Chapter
{
    None = 0,
    Chapter1 = 1,
    Chapter2 = 2,
    Chapter3 = 3
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("--- Audio Source ---")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("--- Music Clips ---")]
    public AudioClip chapter1Music;
    public AudioClip chapter2Music;
    public AudioClip chapter3Music;

    [Header("--- SFX Clips ---")]
    public AudioClip death;
    public AudioClip levelComplete;
    public AudioClip roomComplete;
    public AudioClip specialAbility;

    private Chapter currentChapter = Chapter.None;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.loop = true;
        musicSource.playOnAwake = false;
    }

    // 🎵 Background Music
    public void PlayBackgroundMusic(int levelIndex)
    {
        Chapter newChapter = GetChapterFromLevel(levelIndex);

        if (newChapter == currentChapter && musicSource.isPlaying)
            return;

        currentChapter = newChapter;
        musicSource.clip = GetMusicForChapter(newChapter);
        musicSource.Play();
    }

    private Chapter GetChapterFromLevel(int levelIndex)
    {
        if (levelIndex >= 1 && levelIndex <= 3) return Chapter.Chapter1;
        if (levelIndex >= 4 && levelIndex <= 6) return Chapter.Chapter2;
        if (levelIndex >= 7 && levelIndex <= 9) return Chapter.Chapter3;
        return Chapter.None;
    }

    private AudioClip GetMusicForChapter(Chapter chapter)
    {
        switch (chapter)
        {
            case Chapter.Chapter1: return chapter1Music;
            case Chapter.Chapter2: return chapter2Music;
            case Chapter.Chapter3: return chapter3Music;
            default: return null;
        }
    }

    public void RestartMusic()
    {
        if (musicSource.clip == null) return;
        musicSource.Stop();
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
            musicSource.UnPause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        currentChapter = Chapter.None;
    }
}
