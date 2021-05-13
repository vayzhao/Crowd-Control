using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [Header("Audio Source")]
    [Tooltip("Sorce that plays the sound effects")]
    public AudioSource player;

    [Header("Audio Clip")]
    public AudioClip correctSfx;
    public AudioClip incorrectSfx;
    public AudioClip victorySfx;
    public AudioClip gameoverSfx;

    /// <summary>
    /// Method to play a single clip 
    /// for one shot
    /// </summary>
    /// <param name="whichClip"></param>
    public void Play(AudioClip whichClip)
    {
        player.PlayOneShot(whichClip);
    }

}
