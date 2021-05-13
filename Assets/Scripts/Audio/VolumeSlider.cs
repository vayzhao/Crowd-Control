using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Text sliderText;
    public AudioType audioType;
    private Slider slider;

    private void Start()
    {
        slider = this.GetComponent<Slider>();
        UpdateValueText(slider.value);
    }

    public void UpdateValueText(float newValue)
    {
        sliderText.text = (newValue * 100.05f).ToString("N0")+"%";
        AudioPlayer.EditVolume(audioType, newValue);
    }
}
