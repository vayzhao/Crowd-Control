using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageManager : MonoBehaviour
{
    [Header("Currency")]
    [Tooltip("Initial income to start with")]
    public int initialIncome = 30;
    [Tooltip("Inital income goal aiming to acheive")]
    public int initialGoal = 45;
    [Tooltip("Score to gain when making a right decision")]
    public int reward = 2;
    [Tooltip("Score to lose when making a mistake")]
    public int penalty = -2;
    [Tooltip("Text to display current income")]
    public TextMeshProUGUI txCurrentIncome;
    [Tooltip("Text to display current income goal")]
    public TextMeshProUGUI txCurrentIncomeGoal;

    [Header("Spawning")]
    [Tooltip("The number of spawning patrons")]
    public int spawnQty = 10;    
    [Tooltip("Text to display the remaining number " +
        "of patrons you will need to deal with")]
    public TextMeshProUGUI txRemaining;
    [Tooltip("An empty game object that holds decision buttons")]
    public GameObject decisionBtns;

    [Header("Licence")]
    [Tooltip("Gameobject that represents patron's licence")]
    public GameObject licenceObject;

    [Header("End Game UI")]
    [Tooltip("An empty gameobject that holds all the end game" +
        " ui components")]
    public GameObject endGameHolder;
    [Tooltip("Things to display when the player wins")]
    public GameObject endGameVictory;
    [Tooltip("Things to display when the player loses")]
    public GameObject endGameGameover;

    [Header("Sound Effect")]
    [Tooltip("An empty game object that has all sfx to play")]
    public SoundEffect sfxManager;






    private PatronSpawner spawner;    
    private int remainingSpawn;
    [HideInInspector]
    public bool readyToSpawn;
    private int currentIncome;
    private int currentGoal;
    private RuleManager ruleManager;
    private Patron currentPatron;
    private PatronInformation patronInfo;
    private Vector3 licencePosOrigin;
    private Vector3 licenceEulOrigin;

    // Start is called before the first frame update
    void Start()
    {
        // find the patron spawning script
        spawner = this.GetComponent<PatronSpawner>();

        // find the rule manager script
        ruleManager = this.GetComponent<RuleManager>();

        // record licence original position and rotation
        licencePosOrigin = licenceObject.transform.position;
        licenceEulOrigin = licenceObject.transform.eulerAngles;

        // reset spawn remaining
        ResetRemaining();

        // reset income goal
        ResetIncomeGoal();

        // initialize UI
        InitializeUI();
    }

    /// <summary>
    /// Reset spawn remaining when a new night starts
    /// </summary>
    void ResetRemaining()
    {
        readyToSpawn = true;
        remainingSpawn = spawnQty;
    }

    /// <summary>
    /// Reset income goal when a new night starts
    /// </summary>
    void ResetIncomeGoal()
    {
        currentIncome = initialIncome;
        currentGoal = initialGoal;
    }

    /// <summary>
    /// Method to intialize UI texts everytime when
    /// a new night starts
    /// </summary>
    void InitializeUI()
    {
        txCurrentIncome.text = currentIncome.ToString();
        txCurrentIncomeGoal.text = currentGoal.ToString();
        txRemaining.text = remainingSpawn + " left";
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawning();
    }

    /// <summary>
    /// Method to run every frame, checking to see if 
    /// it is ready to spawn a patron, spawning patrons 
    /// until the remaining number reaches 0, then check
    /// win / lose condition
    /// </summary>
    void CheckSpawning()
    {
        // check to see if it is ready to spawn 
        if (readyToSpawn)
        {
            // check to see if there is more patron to spawn
            if (remainingSpawn > 0)
            {
                // decrease remaining spawning number
                remainingSpawn--;

                // update ui text
                txRemaining.text = (remainingSpawn + 1) + " left";

                // spawn a patron and record its script
                currentPatron = spawner.Spawn();
                patronInfo = currentPatron.gameObject.GetComponent<PatronInformation>();
            }
            // if not, call it a day
            else
            {
                // update ui text
                txRemaining.text = "Completed";

                // determine whether is winning or losing
                CheckWinLose();
            }
        }
    }

    /// <summary>
    /// Method to determine whether the player is winning
    /// or losing by comparing the currentIncome and incomeGoal
    /// </summary>
    void CheckWinLose()
    {
        // comparing current income and current goal
        var isWinning = currentIncome >= currentGoal;

        // show the end game UI
        endGameHolder.SetActive(true);

        // if it is winning 
        if (isWinning)
        {
            // display ui elements and play sfx
            endGameVictory.SetActive(true);
            sfxManager.Play(sfxManager.victorySfx);

        }
        else
        {
            // display ui elements
            endGameGameover.SetActive(true);
            sfxManager.Play(sfxManager.gameoverSfx);
        }
    }

    /// <summary>
    /// Method to display / hide decision buttons
    /// </summary>
    /// <param name="isOn"></param>
    public void DisplayDecisionComponents(bool isOn)
    {
        // display / hide the button objects and licence
        decisionBtns.SetActive(isOn);
        licenceObject.SetActive(isOn);

        // if it is displaying, reset information on licence object
        if (isOn)
        {
            // reset licence's position and rotation
            licenceObject.transform.position = licencePosOrigin;
            licenceObject.transform.eulerAngles = licenceEulOrigin;
        }
        // if it is hiding, then get the next spawning ready
        else
        {
            readyToSpawn = true;
        }
    }

    /// <summary>
    /// Method to update income value, it is called 
    /// after a patron is dealt 
    /// </summary>
    /// <param name="increment"></param>
    public void UpdateIncome(int increment)
    {
        // update current income value
        currentIncome = Mathf.Clamp(currentIncome + increment, 0, currentGoal);

        // refresh ui text
        txCurrentIncome.text = currentIncome.ToString();
    }

    /// <summary>
    /// Accept the current patron to enter the club
    /// </summary>
    public void Accept()
    {
        // check correctness and calculate score
        var correctness = ruleManager.IsPatronEligible(patronInfo);

        // compute result
        Deal(correctness);

        // accept the patron
        currentPatron.Accept();
    }

    /// <summary>
    /// Reject the current patron to enter the club
    /// </summary>
    public void Reject()
    {
        // check correctness and calculate score
        var correctness = !ruleManager.IsPatronEligible(patronInfo);

        // compute result
        Deal(correctness);

        // rejct the patron
        currentPatron.Reject();        
    }

    /// <summary>
    /// Method to call everytime when player
    /// has dealt with a patron
    /// </summary>
    void Deal(bool correctness)
    {
        // calculate score and update income
        var score = correctness ? reward : penalty;
        UpdateIncome(score);

        // play sound effect
        sfxManager.Play(correctness ? sfxManager.correctSfx : sfxManager.incorrectSfx);

        // hide the decision buttons and licence
        DisplayDecisionComponents(false);
    }

    /// <summary>
    /// Method to go back to home page
    /// </summary>
    public void ReturnToHome()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
