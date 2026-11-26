using UnityEngine;

/// <summary>
/// Put one of these in each scene (menu, Level1, Level2, credits) and assign that
/// scene's track. When the scene starts it tells the AudioManager to switch music.
/// If the track is already playing (e.g. two levels share a song), it keeps going without restarting.
/// </summary>
public class SceneMusic : MonoBehaviour
{
    public AudioClip music;

    void Start()
    {
        AudioManager.Instance?.PlayMusic(music);
    }
}
