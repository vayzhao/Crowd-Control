using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Suburb
{
    Kelvin_Grove,
    West_End,
    Kangaroo_Point,
    Woolloongabba,
    Petrie,
    Carseldine,
    Zilmere,
    Ipswich,
    South_Bank,
    Fortitude_Valley
}

public enum Month
{
    Jan,
    Feb,
    Mar,
    Apr,
    May,
    Jun,
    Jul,
    Aug,
    Sep,
    Oct,
    Nov,
    Dec
}

[Serializable]
public enum Colors
{
    None,
    Black,
    Blue,
    Brown,
    Green,
    Purple,
    Red,
    White,
    Yellow
}

[Serializable]
public enum Gender
{
    Male,
    Female
}

public class Info : MonoBehaviour {

    private string[] surnames = new string[20]
    {
        "Mitchell",
        "Rice",
        "Donnelly",
        "Reid",
        "La Selva",
        "Jaffar",
        "Innes",
        "Anderson",
        "Bates",
        "Williams",
        "Bailey",
        "Tran",
        "Brown",
        "McMillan",
        "Nixon",
        "Matell",
        "Mcfly",
        "Simpson",
        "Barker",
        "Wicks"
    };
    private string[] maleNames = new string[15]
    {
        "Dylan",
        "Erik",
        "Dean",
        "Harry",
        "Logan",
        "Christopher",
        "Michael",
        "Riley",
        "Ross",
        "Jack",
        "Daniel",
        "Brock",
        "Mark",
        "David",
        "Tyler"
    };
    private string[] femaleNames = new string[15]
    {
        "Ashley",
        "Sophia",
        "Adriene",
        "Summer",
        "Montana",
        "Sylvia",
        "Jessica",
        "Rebecca",
        "Anne",
        "Carol",
        "Norah",
        "Ruby",
        "Penny",
        "Blake",
        "Maxine"
    };

    public int GetRandomInt(int lower, int upper)
    {
        return UnityEngine.Random.Range(lower, upper);
    }

    public string GenerateSurname()
    {
        return surnames[GetRandomInt(0, surnames.Length)];
    }

    public string GenerateFirstname(Gender whichGender)
    {
        switch (whichGender)
        {
            case Gender.Male:
                return maleNames[GetRandomInt(0, maleNames.Length)];
            case Gender.Female:
                return femaleNames[GetRandomInt(0, femaleNames.Length)];
            default:
                return "";
        }
    }    
}

public class PatronInformation : Info
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
    [Tooltip("Color for the patron's shirt")]
    public Colors[] shirtColor = new Colors[1] { Colors.None };
    [Tooltip("Color for the patron's dress")]
    public Colors[] dressColor = new Colors[1] { Colors.None };
    [Tooltip("Color for the patron's pant")]
    public Colors[] pantColor = new Colors[1] { Colors.None };
    [Tooltip("Color for the patron's shoes")]
    public Colors[] shoesColor = new Colors[1] { Colors.None };

    [Header("Other")]
    public bool hasTie;
    public bool hasHat;
    public bool hasGlasses;

    /// <summary>
    /// Information for the patron
    /// </summary>
    [HideInInspector]
    public string myDOB;
    [HideInInspector]
    public string mySurname;
    [HideInInspector]
    public string myFirstname;
    [HideInInspector]
    public Suburb mySuburb;

    /// <summary>
    /// Method to generate a patron's date 
    /// of birth, surname, firstname and suburb
    /// </summary>
    public void Setup()
    {
        myDOB = CalculateDOB();
        mySurname = GenerateSurname();
        myFirstname = GenerateFirstname(gender);
        mySuburb = (Suburb)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Suburb)).Length);

        //PrintData();
    }

    /// <summary>
    /// Method to calculate a patron's DOB 
    /// </summary>
    /// <returns></returns>
    string CalculateDOB()
    {
        // randomly pick a day and a month
        var day = GetRandomInt(1, 29);
        var month = GetRandomInt(1, 13);

        // calculate year of birth
        var year = 0;
        if (month < Const.month)
        {
            year = Const.year - myAge;
        }
        else if (month > Const.month)
        {
            year = Const.year - (myAge + 1);
        }
        else
        {
            if (day <= Const.day)
            {
                year = Const.year - myAge;
            }
            else
            {
                year = Const.year - (myAge + 1);
            }
        }

        // return the result
        return day.ToString("00") + "/" 
            + ((Month)month - 1).ToString() + "/"
            + (year);
    }

    /// <summary>
    /// Method to print patron's data into console window,
    /// it is used for debuging purpose
    /// </summary>
    void PrintData()
    {
        Debug.Log(string.Format("MyAge:{0} MySuburb:{1}", myAge, mySuburb));
    }
}
