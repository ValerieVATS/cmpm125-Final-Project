using UnityEngine;

/// <summary>
/// One place to play music and sound effects from anywhere:
///   AudioManager.Instance?.PlaySFX(starChime);
/// Persists across scenes. Needs two AudioSource components assigned
/// (one for music with Loop on, one for SFX).
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    /// <summary>Switches music track; does nothing if that track is already playing.</summary>
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null || musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMusicVolume(float v) { if (musicSource) musicSource.volume = v; }
    public void SetSFXVolume(float v) { if (sfxSource) sfxSource.volume = v; }
}
