using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable]
public enum AvatarPart
{
    Hair,
    Hat,
    Shirt,
    Dress,
    Pant,
    Shoe,
    Shoelace
}

[Serializable]
public enum AgeCondition
{
    smaller,
    greater
}

public enum Condition
{
    hasTie,
    hasGlasses
}

[Serializable]
public class Rule
{
    public string tips;
    public Sprite sprite;
}

[Serializable]
public class SingleColorRule : Rule
{
    public AvatarPart avatarPart;
    public Colors value;
}

[Serializable]
public class CombinationColorRule : Rule
{
    public AvatarPart avtarPartA;
    public Colors valueA;
    public AvatarPart avtarPartB;    
    public Colors valueB;
}

[Serializable]
public class AgeRule : Rule
{
    public AgeCondition condition;
    public int value;
}

[Serializable]
public class SuburbRule : Rule
{
    public Suburb value;
}

[Serializable]
public class OtherRule: Rule
{
    public Condition value;
}

[Serializable]
public class RuleBoard {
    public Image image;
    public TextMeshProUGUI text;
}

public class RuleManager : MonoBehaviour
{
    [Header("UI Component")]
    [Tooltip("Components to display rule's icon and text")]
    public RuleBoard[] ruleBoard;

    [Header("All Rules")]
    [Tooltip("Rules for single avatar banning color")]
    public SingleColorRule[] singleColorRule;
    [Tooltip("Rules for banning avatar color combinatoin")]
    public CombinationColorRule[] combinationColorRule;
    [Tooltip("Rules for age restriction")]
    public AgeRule[] ageRule;
    [Tooltip("Rules for suburb restriction")]
    public SuburbRule[] suburbRule;
    [Tooltip("Rules for other material restriction ")]
    public OtherRule[] otherRule;

    /// <summary>
    /// Necessary variable for refreshing the rules
    /// </summary>
    private int allRuleCount;
    private int singleEnd;
    private int combinationEnd;
    private int ageEnd;
    private int suburbEnd;
    private int otherEnd;
    private List<int> remainRules;
    private List<int> currentRules;
    private PatronInformation info;

    void Start()
    {
        GetRuleArrayLengths();
        ResetRemainingRuleIndex();
        UpdateRuleBoards();
    }

    /// <summary>
    /// Method to get the last position index of 
    /// each rules array and the total number of 
    /// the rules
    /// </summary>
    void GetRuleArrayLengths()
    {
        // initialize int lists
        remainRules = new List<int>();
        currentRules = new List<int>();

        // last element position for single color rules
        singleEnd = singleColorRule.Length;

        // last element position for combination color rules
        combinationEnd = singleEnd + combinationColorRule.Length;

        // last element position for age rules
        ageEnd = combinationEnd + ageRule.Length;

        // last element position for suburb rules
        suburbEnd = ageEnd + suburbRule.Length;

        // last element position for other rules
        otherEnd = suburbEnd + otherRule.Length;

        // total number of the rules
        allRuleCount = otherEnd;
    }

    /// <summary>
    /// Method to reset remaining rule indexs
    /// </summary>
    void ResetRemainingRuleIndex()
    {
        remainRules.Clear();
        for (int i = 0; i < allRuleCount; i++)
            remainRules.Add(i);
    }

    /// <summary>
    /// Method to update rules
    /// </summary>
    public void UpdateRuleBoards()
    {
        currentRules.Clear();
        for (int i = 0; i < 6; i++)
            UpdateRuleBoardSingle(i);
    }

    /// <summary>
    /// Method to update a single rule and 
    /// its associated UI component
    /// </summary>
    void UpdateRuleBoardSingle(int ruleBoardIndex)
    {
        // pop up a random value from the remanining
        var index = remainRules[UnityEngine.Random.Range(0, remainRules.Count)];

        // remove the index from remaning and add it to current
        remainRules.Remove(index);
        currentRules.Add(index);

        // find out which rule the index belongs to, and 
        // update its associated UI component
        if (index < singleEnd)
        {
            ruleBoard[ruleBoardIndex].text.text = singleColorRule[index].tips;
            ruleBoard[ruleBoardIndex].image.sprite = singleColorRule[index].sprite;
        }
        else if (index < combinationEnd)
        {
            ruleBoard[ruleBoardIndex].text.text = combinationColorRule[index - singleEnd].tips;
            ruleBoard[ruleBoardIndex].image.sprite = combinationColorRule[index - singleEnd].sprite;
        }
        else if (index < ageEnd)
        {
            ruleBoard[ruleBoardIndex].text.text = ageRule[index - combinationEnd].tips;
            ruleBoard[ruleBoardIndex].image.sprite = ageRule[index - combinationEnd].sprite;
        }
        else if (index < suburbEnd)
        {
            ruleBoard[ruleBoardIndex].text.text = suburbRule[index - ageEnd].tips;
            ruleBoard[ruleBoardIndex].image.sprite = suburbRule[index - ageEnd].sprite;
        }
        else 
        {
            ruleBoard[ruleBoardIndex].text.text = otherRule[index - suburbEnd].tips;
            ruleBoard[ruleBoardIndex].image.sprite = otherRule[index - suburbEnd].sprite;
        }

        // when running out of remaining, reset it
        if (remainRules.Count == 0)
            ResetRemainingRuleIndex();
    }

    /// <summary>
    /// Method to check eligibility for patrons, running through
    /// each rule and check if the patron has any component against
    /// the rule.
    /// </summary>
    public bool IsPatronEligible(PatronInformation patronInfo)
    {
        // initially, set eligibility to be true, and
        // bind the checked patron to this script
        var eligibility = true;
        info = patronInfo;

        // run through all elements in current rules,
        // try to compare the rule and patronInfo to 
        // see if the patron is eligible to enter
        for (int i = 0; i < currentRules.Count && eligibility; i++)
        {
            // single color rules
            if (currentRules[i] < singleEnd) { 
                eligibility = CheckSingleColorRule(currentRules[i]);

                if (!eligibility)
                {
                    Debug.Log("SingleColor rule fail");
                }
            }
            // combination color rules
            else if (currentRules[i] < combinationEnd) { 
                eligibility = CheckCombiantionColorRule(currentRules[i] - singleEnd);

                if (!eligibility)
                {
                    Debug.Log("CombinationColor rule fail");
                }
            }
            // age rules
            else if (currentRules[i] < ageEnd) { 
                eligibility = CheckAgeRule(currentRules[i] - combinationEnd);

                if (!eligibility)
                {
                    Debug.Log("Age rule fail");
                }
            }
            // suburb rules
            else if (currentRules[i] < suburbEnd) { 
                eligibility = CheckSuburbRule(currentRules[i] - ageEnd);

                if (!eligibility)
                {
                    Debug.Log("Suburb rule fail");
                }
            }
            // other rules
            else { 
                eligibility = CheckOtherRule(currentRules[i] - suburbEnd);

                if (!eligibility)
                {
                    Debug.Log("Other rule fail");
                }
            }
        }
        return eligibility;
    }

    /// <summary>
    /// Methods to check to see if the specific part of avatar that 
    /// patron is wearing does not contain the banned color
    /// </summary>
    /// <returns></returns>
    bool CheckSingleColorRule(int index)
    {
        // get the checked avatar part and the banned color
        var ava = singleColorRule[index].avatarPart;
        var ban = singleColorRule[index].value;

        return IsAvatarEligible(ava, ban);
    }
    bool CheckCombiantionColorRule(int index)
    {
        // get the first check avatart part and its banned color
        var avaA = combinationColorRule[index].avtarPartA;
        var banA = combinationColorRule[index].valueA;
        var avaB = combinationColorRule[index].avtarPartB;
        var banB = combinationColorRule[index].valueB;

        return IsAvatarEligible(avaA, banA) || IsAvatarEligible(avaB, banB);
    }

    /// <summary>
    /// Method to check to see if the specific part of avatar
    /// does not contain the banned color
    /// </summary>
    /// <returns></returns>
    bool IsAvatarEligible(AvatarPart whichPart, Colors ban)
    {
        switch (whichPart)
        {
            case AvatarPart.Hair:
                return info.hairColor != ban;
            case AvatarPart.Hat:
                return !IsContained(info.hatColor, ban);
            case AvatarPart.Shirt:
                return !IsContained(info.shirtColor, ban);
            case AvatarPart.Dress:
                return !IsContained(info.dressColor, ban);
            case AvatarPart.Pant:
                return !IsContained(info.pantColor, ban);
            case AvatarPart.Shoe:
                return !IsContained(info.shoesColor, ban);
            case AvatarPart.Shoelace:
                return info.shoelaceColor != ban;
            default:
                return true;
        }
    }

    /// <summary>
    /// Method to check to see if the patron's age is either 
    /// smaller or greater than a specific age
    /// </summary>
    /// <returns></returns>
    bool CheckAgeRule(int index)
    {
        switch (ageRule[index].condition)
        {
            case AgeCondition.smaller:
                return info.myAge > ageRule[index].value;
            case AgeCondition.greater:
                return info.myAge < ageRule[index].value;
            default:
                return true;
        }
    }

    /// <summary>
    /// Method to check to see if the patron is not from
    /// the specifc suburb
    /// </summary>
    /// <returns></returns>
    bool CheckSuburbRule(int index)
    {
        return info.mySuburb != suburbRule[index].value;
    }

    /// <summary>
    /// Method to check to see if the patron does not 
    /// contain the specific material (tie, sun glasses)
    /// </summary>
    /// <returns></returns>
    bool CheckOtherRule(int index)
    {
        switch (otherRule[index].value)
        {
            case Condition.hasTie:
                return !info.hasTie;
            case Condition.hasGlasses:
                return !info.hasGlasses;
            default:
                return true;
        }
    }

    /// <summary>
    /// check through an colors array and return true if the 
    /// array contains the given banning color
    /// </summary>
    /// <returns></returns>
    bool IsContained(Colors[] colors, Colors ban)
    {
        // check to see if ban color is in the array
        foreach (var color in colors)
            if (color == ban)
                return true;

        // otherwise returns false
        return false;
    }
}
