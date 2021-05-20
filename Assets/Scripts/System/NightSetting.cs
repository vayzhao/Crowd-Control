using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [HideInInspector]
    public bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlider();
        ResetTimer();        
    }

    // Update is called once per frame
    void Update()
    {
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
    /// Method to reset timer slider and calendar
    /// </summary>
    void ResetTimer()
    {
        timerSlider.value = timerSlider.maxValue;
        txCalender.text = Const.day.ToString("00") + "/" + 
            Const.month.ToString("00") + "/" + Const.year;
        isRunning = true;
    }
    
    /// <summary>
    /// Method to run the timer every frame
    /// </summary>
    void ReducingTimer()
    {
        if (timerSlider.value>0)
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
        txStage.text = "Stage 1";
    }

    /// <summary>
    /// Method to start next night
    /// </summary>
    public void StartANewNight(int n)
    {
        Const.NextDay(n);
        ResetTimer();
        txStage.text = string.Format("Stage {0}", n+1);
    }
}
