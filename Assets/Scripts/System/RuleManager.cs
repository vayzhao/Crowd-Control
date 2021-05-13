using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum ConditionType
{
    Hair,
    Hat,
    Shirt,
    Pant,
    Shoes,
    Shoelace,
    AgeSmaller,
    AgeGreater
}

[Serializable]
public class Rule
{
    public ConditionType type;
    public Colors conditionColor;
    public int conditionAge;
}

public class RuleManager : MonoBehaviour
{
    [Header("Conditions")]
    [Tooltip("Banning condition")]
    public Rule[] rules = new Rule[6];

    /// <summary>
    /// Method to check eligibility for patrons, running through
    /// each rule and check if the patron has any component against
    /// the rule.
    /// </summary>
    /// <returns></returns>
    public bool IsPatronEligible(PatronInformation patronInfo)
    {
        // initially, set eligibility to be true
        var eligibility = true;

        // check through each rule and see if the patron
        // information is not against the rule
        for (int i = 0; i < rules.Length; i++)
        {
            switch (rules[i].type)
            {
                case ConditionType.Hair:
                    eligibility = patronInfo.hairColor != rules[i].conditionColor;
                    break;
                case ConditionType.Hat:
                    eligibility = !IsContained(patronInfo.hatColor, rules[i].conditionColor);
                    break;
                case ConditionType.Shirt:
                    eligibility = !IsContained(patronInfo.shirtColor, rules[i].conditionColor);
                    break;
                case ConditionType.Pant:
                    eligibility = !IsContained(patronInfo.pantColor, rules[i].conditionColor);
                    break;
                case ConditionType.Shoes:
                    eligibility = !IsContained(patronInfo.shoesColor, rules[i].conditionColor);
                    break;
                case ConditionType.Shoelace:
                    eligibility = patronInfo.shoelaceColor != rules[i].conditionColor;
                    break;
                case ConditionType.AgeSmaller:
                    eligibility = patronInfo.myAge <= rules[i].conditionAge;
                    break;
                case ConditionType.AgeGreater:
                    eligibility = patronInfo.myAge >= rules[i].conditionAge;
                    break;
                default:
                    break;
            }

            // if we find any component of the patron is against the rule, break the forloop
            if (!eligibility)
                break;
        }

        return eligibility;
    }

    /// <summary>
    /// check through an colors array and return true if the 
    /// array contains the given banning color
    /// </summary>
    /// <returns></returns>
    bool IsContained(Colors[] patronComponentColors, Colors banningColor)
    {
        // check to see if banning color is contained in the array
        for (int i = 0; i < patronComponentColors.Length; i++)
            if (patronComponentColors[i]==banningColor)
                return true;

        // return false if can't find any
        return false;
    }


}
