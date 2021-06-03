using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType{
    Music,
    Effect,
    Environment,
    Speech
}

public class AudioPlayer : MonoBehaviour
{
    const float VOLUME_MUTE = -80f;
    const float VOLUME_MIN = -10f;
    const float VOLUME_MAX = 10f;
    public const float DEFAULT_VOL_MUSIC = 0.6f;
    public const float DEFAULT_VOL_EFFECT = 0.35f;
    public const float DEFAULT_VOL_ENVIRONMENT = 0.10f;
    public const float DEFAULT_VOL_SPEECH = 0.8f;

    public static float musicVolume;
    public static float effectVolume;
    public static float environmentVolume;
    public static float speechVolume;

    [Header("Clip Files")]
    public AudioClip defaultBgm;
    public AudioClip[] environmentBgms;
    public static AudioSource srcMusic;
    public static AudioSource srcEffect;
    public static AudioSource srcEnvironment;
    public static AudioSource srcSpeech;
    public static AudioMixer audioMixer;

    private List<int> environmentBgmIndexs;

    // Start is called before the first frame update
    void Start()
    {
        // get the audio source components from children
        srcMusic = this.transform.GetChild(0).GetComponent<AudioSource>();
        srcEffect = this.transform.GetChild(1).GetComponent<AudioSource>();
        srcEnvironment = this.transform.GetChild(2).GetComponent<AudioSource>();
        srcSpeech = this.transform.GetChild(3).GetComponent<AudioSource>();

        // find the audio mixer and update volume
        audioMixer = srcMusic.outputAudioMixerGroup.audioMixer;
        musicVolume = DEFAULT_VOL_MUSIC;
        effectVolume = DEFAULT_VOL_EFFECT;
        environmentVolume = DEFAULT_VOL_ENVIRONMENT;
        speechVolume = DEFAULT_VOL_SPEECH;
        EditVolume(AudioType.Music, musicVolume);
        EditVolume(AudioType.Effect, effectVolume);
        EditVolume(AudioType.Environment, environmentVolume);
        EditVolume(AudioType.Speech, speechVolume);

        // setup environment bgm index
        environmentBgmIndexs = new List<int>();
        ResetEnvironmentBgmIndex();

        // play the default background music
        PlayClip(AudioType.Music, defaultBgm);
    }

    /// <summary>
    /// Method to play environment bgm and refresh its
    /// index list.
    /// </summary>
    public void RefreshEnvironmentBGM()
    {
        // reset environment index list if it is empty
        if (environmentBgmIndexs.Count == 0)
            ResetEnvironmentBgmIndex();

        // pick a random index and remove it from the list
        var index = Random.Range(0, environmentBgmIndexs.Count);
        environmentBgmIndexs.Remove(index);

        // play the clip
        PlayClip(AudioType.Environment, environmentBgms[index]);
    }
    void ResetEnvironmentBgmIndex()
    {
        environmentBgmIndexs.Clear();
        for (int i = 0; i < environmentBgms.Length; i++)
            environmentBgmIndexs.Add(i);
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
            case AudioType.Environment:
                srcEnvironment.clip = clip;
                srcEnvironment.Play();
                break;
            case AudioType.Speech:
                srcSpeech.Stop();
                srcSpeech.clip = clip;
                srcSpeech.Play();
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
            case AudioType.Environment:
                audioMixer.SetFloat("Environment", value);
                break;
            case AudioType.Speech:
                audioMixer.SetFloat("Speech", value);
                break;
            default:
                break;
        }
    }
}
