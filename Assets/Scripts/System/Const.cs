using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Const : MonoBehaviour
{
    // current game time
    public static int year;
    public static int month;
    public static int day;

    // controller that pressing decision buttons
    public static Controller leftController;
    public static Controller rightController;
    public static Controller pressingController;

    // patron number increase by n per night
    public const int WAVE_QUANTITY_INCREASE = 1;

    // distance to determine whether an object reaches a position
    public const float DISTANCE_REACH = 0.1f;

    /// <summary>
    /// Method to get current date when
    /// finished loading
    /// </summary>
    public static void SetupDate()
    {
        // get current date
        var current = DateTime.Now;

        // store date data into year, month and day
        year = current.Year;
        month = current.Month;
        day = current.Day;
    }
    public static void NextDay(int n)
    {
        // increase current date by adding n day
        var nextDay = DateTime.Now.AddDays(n);

        // store date data into year, month and day
        year = nextDay.Year;
        month = nextDay.Month;
        day = nextDay.Day;
    }

}
