using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const : MonoBehaviour
{
    // current game time
    public static int year = 2021;
    public static int month = 5;
    public static int day = 14;

    // patron number increase by n per night
    public const int WAVE_QUANTITY_INCREASE = 1;

    // distance to determine whether an object reaches a position
    public const float DISTANCE_REACH = 0.1f;
}
