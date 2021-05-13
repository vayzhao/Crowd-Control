using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum Colors
{
    None,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Cyan,
    Purple,
    White,
    Black,
    Brown,
    Gray,
    Pink
}

[Serializable]
public enum Gender
{
    Male,
    Female
}

public class PatronInformation : MonoBehaviour
{
    [Header("Personal Information")]
    [Tooltip("Gender for the patron, this setting will" +
        "affect name generating")]
    public Gender gender;
    [Tooltip("Age for the patron")]
    public int myAge;

    [Header("Colors")]
    [Tooltip("Color for the patron's hair")]
    public Colors hairColor;
    [Tooltip("Color for the patron's hat")]
    public Colors[] hatColor = new Colors[1] { Colors.None };
    [Tooltip("Color for the patron's shirt")]
    public Colors[] shirtColor = new Colors[1] { Colors.None };
    [Tooltip("Color for the patron's pant")]
    public Colors[] pantColor = new Colors[1] { Colors.None };
    [Tooltip("Color for the patron's shoes")]
    public Colors[] shoesColor = new Colors[1] { Colors.None };
    [Tooltip("Color for the patron's shoelaces")]
    public Colors shoelaceColor;

    [Header("Other")]
    [Tooltip("Determine whether the patron is wearing a watch")]
    public bool hasWatch;
    [Tooltip("Determine whether the patron is wearing glasses")]
    public bool hasGlasses;

    // name for the patron
    private string myName;

    public void GenerateName()
    {
        this.myName = "A person";
    }
}
