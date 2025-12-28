using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXPrefab;

    private void Awake()
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

    public void PlaySoundFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        AudioSource source = Instantiate(soundFXPrefab, position, Quaternion.identity);
        source.clip = clip;
        source.volume = volume;
        source.Play();

        Destroy(source.gameObject, clip.length);
    }
}
