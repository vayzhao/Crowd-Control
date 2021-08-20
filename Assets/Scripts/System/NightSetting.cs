using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class SkySet
{
    public Material skyBoxMat;
    public Color skyColor;
}

public class NightSetting : MonoBehaviour
{
    [Header("Date")]
    [Tooltip("Text to display current date")]
    public TextMeshProUGUI txCalender;
    [Tooltip("Text to display current stage")]
    public TextMeshProUGUI txStage;
    [Tooltip("Slider that tells you remaining time")]
    public Slider timerSlider;
    
    [Header("Mechanic")]
    [Tooltip("How many second per night lasts")]
    public int perNightLast = 60;

    [Header("Skyboxes")]
    [Tooltip("Sky Boxes Prefabs")]
    public SkySet[] skySets;
    private List<int> skySetIndex;

    [HideInInspector]
    public bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        skySetIndex = new List<int>();

        InitializeSlider();
        ResetTimer();
        ResetSkyIndex();

        ChangeSky();
    }

    // Update is called once per frame
    void Update()
    {
        RotateSkyBox();
        ReducingTimer();
    }

    /// <summary>
    /// Method to setup the timer slider
    /// </summary>
    void InitializeSlider()
    {
        timerSlider.minValue = 0;
        timerSlider.maxValue = perNightLast;
    }

    /// <summary>
    /// Method to reset sky index
    /// </summary>
    void ResetSkyIndex()
    {
        skySetIndex.Clear();
        for (int i = 0; i < skySets.Length; i++)
            skySetIndex.Add(i);
    }

    /// <summary>
    /// Method to update the sky
    /// </summary>
    void ChangeSky()
    {
        // reset the list if the list is empty
        if (skySetIndex.Count == 0)
            ResetSkyIndex();

        // get a random index
        var index = skySetIndex[UnityEngine.Random.Range(0, skySetIndex.Count)];

        // remove this index from the list
        skySetIndex.Remove(index);

        // Update Render Settings
        RenderSettings.skybox = skySets[index].skyBoxMat;
        RenderSettings.ambientSkyColor = skySets[index].skyColor;
    }

    /// <summary>
    /// Method to reset timer slider and calendar
    /// </summary>
    void ResetTimer()
    {
        timerSlider.value = timerSlider.maxValue;
        txCalender.text = Const.dateString;
        isRunning = true;
    }

    /// <summary>
    /// Method to rotate the skybox material
    /// </summary>
    void RotateSkyBox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }

    /// <summary>
    /// Method to run the timer every frame
    /// </summary>
    void ReducingTimer()
    {
        if (timerSlider.value > 0 && !Tutorial.tutorialHoldSpawn) 
        {
            timerSlider.value -= Time.deltaTime;

            if (timerSlider.value <= 0) 
            {
                isRunning = false;
            }
        }
    }

    /// <summary>
    /// Reset the night to intial, it is called 
    /// when the gamr first start or restart
    /// </summary>
    public void ResetNight()
    {
        Const.SetupDate();
        ResetTimer();
        ChangeSky();
        txStage.text = "Stage 1";
    }

    /// <summary>
    /// Method to start next night
    /// </summary>
    public void StartANewNight(int n)
    {
        Const.NextDay(n);
        ChangeSky();
        ResetTimer();
        txStage.text = string.Format("Stage {0}", n+1);
    }
}
