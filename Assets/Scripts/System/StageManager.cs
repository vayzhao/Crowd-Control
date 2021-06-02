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
    public int initialGoal = 40;
    [Tooltip("Score to gain when making a right decision")]
    public int reward = 2;
    [Tooltip("Score to lose when making a mistake")]
    public int penalty = -2;
    [Tooltip("Text to display current income")]
    public TextMeshProUGUI txIncome;

    [Header("Spawning")]
    [Tooltip("An empty game object that holds decision buttons")]
    public GameObject decisionBtns;
    [Tooltip("A object that holds dancing patron")]
    public DanceFloor danceFloor;

    [Header("Licence")]
    [Tooltip("Gameobject that represents patron's licence")]
    public GameObject licenceObject;

    [Header("End Game UI")]
    [Tooltip("Things to display when the player wins")]
    public GameObject endGameVictory;
    [Tooltip("Things to display when the player loses")]
    public GameObject endGameGameover;
    [Tooltip("Things to display InCome & Date")]
    public GameObject inGameUIHolder;
    [Tooltip("Computing Text")]
    public TextMeshProUGUI computingText;
    [Tooltip("A button that allows the player to enter next night")]
    public GameObject nextButton;
    [Tooltip("A button that allows the player to restart the game")]
    public GameObject restartButton;
    [Tooltip("A button that allows the player to exit the game")]
    public GameObject quitButton;

    [Header("Sound Effect")]
    [Tooltip("An empty game object that has all sfx to play")]
    public SoundEffect sfxManager;
    [Tooltip("An empty game object that plays the environment bgm")]
    public AudioPlayer audioPlayer;

    private PatronSpawner spawner;    
    [HideInInspector]
    public bool readyToSpawn;
    private int stage;
    private int currentIncome;
    private int currentGoal;
    private RuleManager ruleManager;
    private NightSetting nightSetting;
    private Patron currentPatron;
    private PatronInformation patronInfo;
    private Vector3 licencePosOrigin;
    private Vector3 licenceEulOrigin;
    private OVRGrabbable licenceGrabbable;

    // Start is called before the first frame update
    void Start()
    {
        // initial stage
        stage = 1;

        // find the patron spawning script
        spawner = this.GetComponent<PatronSpawner>();

        // find the rule manager script
        ruleManager = transform.parent.GetComponentInChildren<RuleManager>();

        // find night setting script
        nightSetting = this.GetComponent<NightSetting>();

        // find licence grabbable script
        licenceGrabbable = licenceObject.GetComponent<OVRGrabbable>();

        // record licence original position and rotation
        licencePosOrigin = licenceObject.transform.position;
        licenceEulOrigin = licenceObject.transform.eulerAngles;

        // reset income goal
        ResetIncomeGoal();

        // refresh environment bgm
        audioPlayer.RefreshEnvironmentBGM();

        // set ready to spawn to be true
        readyToSpawn = true;
    }

    /// <summary>
    /// Reset income goal when a new night starts
    /// </summary>
    void ResetIncomeGoal()
    {
        // reset income and goal to initial values
        currentIncome = initialIncome;
        currentGoal = initialGoal;

        // update text components
        txIncome.text = string.Format("Misson\n{0}/{1}", currentIncome, currentGoal);
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
            // show up result when the player has enough score to win
            // or when the timer has finished
            if (!nightSetting.isRunning || currentIncome >= currentGoal) 
            {
                // disable spawn
                readyToSpawn = false;

                // hide in game ui components
                inGameUIHolder.SetActive(false);

                // computing
                computingText.gameObject.SetActive(true);
                Invoke("CheckWinLose", 3f);
            }
            // othwesie, spawn a patron
            else
            {
                currentPatron = spawner.Spawn();
                patronInfo = currentPatron.GetComponent<PatronInformation>();
            }
        }
    }

    /// <summary>
    /// Method to determine whether the player is winning
    /// or losing by comparing the currentIncome and incomeGoal
    /// </summary>
    void CheckWinLose()
    {
        // hide computing text
        computingText.gameObject.SetActive(false);

        // comparing current income and current goal
        var isWinning = currentIncome >= currentGoal;

        // if it is winning 
        if (isWinning)
        {
            // show the winner option
            nextButton.SetActive(true);
            
            // display ui elements and play sfx
            endGameVictory.SetActive(true);
            sfxManager.Play(sfxManager.victorySfx);

        }
        else
        {
            // show the loser options
            restartButton.SetActive(true);
            quitButton.SetActive(true);

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
        // if it is displaying, reset information on licence object
        if (isOn)
        {
            // reset licence's grabbable
            licenceGrabbable.enabled = true;

            // reset licence's position and rotation
            licenceObject.transform.position = licencePosOrigin;
            licenceObject.transform.eulerAngles = licenceEulOrigin;
        }
        // otherwise...
        else
        {
            // set next spawning to be ready
            readyToSpawn = true;

            // let go the licence object            
            licenceGrabbable.grabbedBy?.ForceRelease(licenceGrabbable);
            licenceGrabbable.enabled = false;

            // reset licence's position and rotation
            licenceObject.transform.position = licencePosOrigin;
            licenceObject.transform.eulerAngles = licenceEulOrigin;

            // reset decision buttons
            foreach (var button in decisionBtns.GetComponentsInChildren<PhysicalButton>())
                button.ResetButton(false);
        }

        // display / hide the button objects and licence
        decisionBtns.SetActive(isOn);
        licenceObject.SetActive(isOn);
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
        txIncome.text = string.Format("Misson\n{0}/{1}", currentIncome, currentGoal);
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

        // vibrate the controllers when making a wrong decision
        if (!correctness)
            Const.pressingController.AddVibration(1f, 1f, 0.5f);
    }
    
    /// <summary>
    /// Method to move onto the next stage
    /// </summary>
    public void Next()
    {
        // clean up dance floor
        danceFloor.Clear();

        // hide option buttons
        inGameUIHolder.SetActive(true);
        nextButton.GetComponentInChildren<PhysicalButton>().ResetButton(true);
        endGameVictory.SetActive(false);

        // reset current income and goal
        currentIncome = currentGoal;
        currentGoal += 10;

        // update text components
        txIncome.text = string.Format("Misson\n{0}/{1}", currentIncome, currentGoal);

        // move to next day
        nightSetting.StartANewNight(stage);

        // increase stage
        stage++;

        // refresh rules 
        ruleManager.UpdateRuleBoards();

        // refresh environment bgm
        audioPlayer.RefreshEnvironmentBGM();

        // set ready to spawn to be true
        readyToSpawn = true;
    }

    /// <summary>
    /// Method to restart the game
    /// </summary>
    public void Restart()
    {
        // clean up dance floor
        danceFloor.Clear();

        // hide option buttons
        inGameUIHolder.SetActive(true);
        restartButton.GetComponentInChildren<PhysicalButton>().ResetButton(true);
        quitButton.GetComponentInChildren<PhysicalButton>().ResetButton(true);
        endGameGameover.SetActive(false);

        // reset income
        ResetIncomeGoal();

        // reset night
        nightSetting.ResetNight();

        // refresh rules 
        ruleManager.UpdateRuleBoards();

        // refresh environment bgm
        audioPlayer.RefreshEnvironmentBGM();

        // set ready to spawn to be true
        readyToSpawn = true;
    }

    /// <summary>
    /// Method to go back to home page
    /// </summary>
    public void ReturnToHome()
    {
        SceneManager.LoadScene(0);
    }
}
