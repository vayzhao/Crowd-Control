using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Licence : MonoBehaviour
{
    [Header("Front Components")]
    [Tooltip("a text component to display" +
        " patron's surname")]
    public TextMeshProUGUI surname;
    [Tooltip("a text component to display" +
        " patron's firstname")]
    public TextMeshProUGUI firstname;
    [Tooltip("a text component to display" +
        " licence number")]
    public TextMeshProUGUI licenceNumber;
    [Tooltip("a text component to display" +
        " patron's DOB")]
    public TextMeshProUGUI DOB;
    [Tooltip("a text component to display" +
        " licence's effective date")]
    public TextMeshProUGUI effective;
    [Tooltip("a text component to display" +
        " licence's expiry date")]
    public TextMeshProUGUI expiry;
    [Tooltip("a text component to display" +
        " patron's signature")]
    public TextMeshProUGUI signature;

    [Header("Back Components")]
    [Tooltip("a text component to display" +
        " patron's suburb")]
    public TextMeshProUGUI address;
    [Tooltip("a text component to display" +
        " card number(horizontal one)")]
    public TextMeshProUGUI cardNumber;
    [Tooltip("a text component to display" +
        " card number(vertical one)")]
    public TextMeshProUGUI cardNumber2;

    [Header("Other")]
    [Tooltip("An empty game object that holds" +
        "the patron preview model and camera")]
    public Transform patronModelHolder;

    /// <summary>
    /// Necessary variables 
    /// </summary>
    private PatronInformation info;
    private GameObject patronModel;
    private Outline outLineFx;
    private OVRGrabbable grabbable;

    void Update()
    {
        CheckOutline();
    }

    /// <summary>
    /// Method to check if the licence nees to 
    /// outline or not. When the licence object
    /// is grabbed, disable outline effect,
    /// otherwise turn it on
    /// </summary>
    void CheckOutline()
    {
        outLineFx.enabled = !grabbable.isGrabbed;
    }

    /// <summary>
    /// Method to refresh datas in the driver's
    /// licence, it is called when switching to
    /// different patrons
    /// </summary>
    /// <param name="info"></param>
    public void Refresh(PatronInformation info)
    {
        // bind patron's infomation
        this.info = info;

        // find core components
        outLineFx = GetComponent<Outline>();
        grabbable = GetComponent<OVRGrabbable>();

        // fill up patron's information
        FillUpInfo();

        // refresh patron's protrait
        RefreshProtrait();

        // generate some irrelevant elements
        GenerateDates();
        GenerateCardNumber();
        GenerateLicenceNumber();
    }

    /// <summary>
    /// Method to fill patron's information
    /// into the licence object
    /// </summary>
    void FillUpInfo()
    {
        DOB.text = info.myDOB;
        surname.text = info.mySurname;
        firstname.text = info.myFirstname;
        address.text = info.mySuburb.ToString();
        signature.text = info.myFirstname + info.mySurname;
    }
    
    /// <summary>
    /// Method to refresh licence protrait image,
    /// </summary>
    void RefreshProtrait()
    {
        // remove the previous patron model
        if (patronModel)
            Destroy(patronModel);

        // instantiate a new patron model 
        patronModel = Instantiate(info.gameObject, patronModelHolder);
        patronModel.transform.localScale = Vector3.one;
        patronModel.transform.localPosition = Vector3.zero;
        patronModel.transform.localEulerAngles = Vector3.zero;

        // remove animation controller from 
        // the patron model
        Destroy(patronModel.GetComponent<Animator>());
    }

    /// <summary>
    /// Method to generate an effective date
    /// and an expiry date
    /// </summary>
    void GenerateDates()
    {
        // calculate effective and expiry
        var effectiveYear = Const.year - info.myAge + 19;
        if (Const.year - effectiveYear > 10)
        {
            effectiveYear = Const.year - Random.Range(3, 6);
        }
        var effectiveMonth = Random.Range(1, 13);
        var effectiveDay = Random.Range(1, 29);
        var expiryYear = effectiveYear + 5;

        // apply these resultes to ui components
        effective.text = effectiveDay + "." + effectiveMonth + "." + effectiveYear;
        expiry.text = effectiveDay + "." + effectiveMonth + "." + expiryYear;
    }

    /// <summary>
    /// Method to generate a random licence number
    /// </summary>
    void GenerateLicenceNumber()
    {
        // intiailize licence number
        var number = "";
        for (int i = 0; i < 11; i++)
        {
            // insert space into the fourth and
            // eighth position
            if (i == 3 || i == 7)
                number += " ";
            // otherwise add a random 1 digit
            // number to the current position
            else
                number += Random.Range(0, 10);
        }

        // apply rsult to licence number text componment
        licenceNumber.text = number;
    }

    /// <summary>
    /// Method to generate random card numbers
    /// </summary>
    void GenerateCardNumber()
    {
        // initialize card numbers1
        var number1 = "";
        for (int i = 0; i < 12; i++)
        {
            // insert space into the fifth and
            // tenth position
            if (i == 4 || i == 9)
                number1 += " ";
            // insert "AD" into the eighth and 
            // nineth position
            else if (i == 7)
                number1 += "A";
            else if (i == 8)
                number1 += "D";
            // otherwise add a random 1 digit 
            // number to the current position
            else
                number1 += Random.Range(0, 10);
        }

        // initialize card numbers2
        var number2 = "";
        for (int i = 0; i < 13; i++)
        {
            // insert space into the eighth position
            if (i == 7)
                number2 += " ";
            // otherwise add a random 1 digit
            // number to the current position
            else
                number2 += Random.Range(0, 10);
        }

        // apply these results to ui components
        cardNumber.text = number1;
        cardNumber2.text = number2;
    }
}
