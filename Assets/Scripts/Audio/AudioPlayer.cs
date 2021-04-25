using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType{
    Music,
    Effect
}

public class AudioPlayer : MonoBehaviour
{
    const float VOLUME_MUTE = -80f;
    const float VOLUME_MIN = -10f;
    const float VOLUME_MAX = 10f;
    public const float DEFAULT_VOL_MUSIC = 0.5f;
    public const float DEFAULT_VOL_EFFECT = 0.5f;

    public static float musicVolume;
    public static float effectVolume;

    public AudioClip defaultBgm;
    public static AudioSource srcMusic;
    public static AudioSource srcEffect;
    public static AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        // get the audio source components from children
        srcMusic = this.transform.GetChild(0).GetComponent<AudioSource>();
        srcEffect = this.transform.GetChild(1).GetComponent<AudioSource>();

        // find the audio mixer and update volume
        audioMixer = srcMusic.outputAudioMixerGroup.audioMixer;
        musicVolume = DEFAULT_VOL_MUSIC;
        effectVolume = DEFAULT_VOL_EFFECT;
        EditVolume(AudioType.Music, musicVolume);
        EditVolume(AudioType.Effect, effectVolume);

        // play the default background music
        PlayClip(AudioType.Music, defaultBgm);
    }

    /// <summary>
    /// Method to play an audio clip
    /// </summary>
    /// <param name="type">the audio type is going to be played</param>
    /// <param name="clip">the audio clip is going to be played</param>
    public static void PlayClip(AudioType type, AudioClip clip)
    {
        switch (type)
        {
            case AudioType.Music:
                srcMusic.clip = clip;
                srcMusic.Play();
                break;
            case AudioType.Effect:
                srcEffect.PlayOneShot(clip);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Method to edit audio mixer's value
    /// </summary>
    /// <param name="type"></param>
    /// <param name="newValue"></param>
    public static void EditVolume(AudioType type, float newValue)
    {
        // translating percentage value to dB value
        var value = newValue == 0f ? VOLUME_MUTE : Mathf.Lerp(VOLUME_MIN, VOLUME_MAX, newValue);

        switch (type)
        {
            case AudioType.Music:
                audioMixer.SetFloat("Music", value);
                break;
            case AudioType.Effect:
                audioMixer.SetFloat("Effect", value);
                break;
            default:
                break;
        }
    }
}
