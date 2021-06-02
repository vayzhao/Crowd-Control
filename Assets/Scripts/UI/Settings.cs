using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject unsavedChangePanel;

    public Slider musicSlider;
    public Slider effectSlider;
    public Slider environmentSlider;

    void Start()
    {
        ResetSettings();
    }

    /// <summary>
    /// Reset everything to default
    /// </summary>
    public void ResetSettings()
    {
        musicSlider.value = AudioPlayer.DEFAULT_VOL_MUSIC;
        effectSlider.value = AudioPlayer.DEFAULT_VOL_EFFECT;
        environmentSlider.value = AudioPlayer.DEFAULT_VOL_ENVIRONMENT;
    }

    /// <summary>
    /// Attempt to exit setting panel from exit button
    /// </summary>
    public void CheckChange()
    {
        // check if anything has been changed
        if (musicSlider.value != AudioPlayer.musicVolume
            || effectSlider.value != AudioPlayer.effectVolume
            || environmentSlider.value != AudioPlayer.environmentVolume)
        {
            unsavedChangePanel.SetActive(true);
        }
        else
        {
            this.GetComponent<Buttons>().Close();
        }
    }

    /// <summary>
    /// Save changes when exiting setting panel
    /// </summary>
    public void Save()
    {
        AudioPlayer.musicVolume = musicSlider.value;
        AudioPlayer.effectVolume = effectSlider.value;
        AudioPlayer.environmentVolume = environmentSlider.value;

        unsavedChangePanel.SetActive(false);
        this.GetComponent<Buttons>().Close();
    }

    /// <summary>
    /// Reset changes when exiting setting panel
    /// </summary>
    public void DontSave()
    {
        musicSlider.value = AudioPlayer.musicVolume;
        effectSlider.value = AudioPlayer.effectVolume;
        environmentSlider.value = AudioPlayer.environmentVolume;

        unsavedChangePanel.SetActive(false);
        this.GetComponent<Buttons>().Close();
    }
}
