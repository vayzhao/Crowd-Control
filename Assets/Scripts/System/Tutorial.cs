using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public static bool isTutorialOn = false;
    public static bool tutorialHoldSpawn;

    public TextMeshProUGUI content;
    public TextMeshProUGUI btnText;
    public AudioClip[] tutorialClips;
    public GameObject uiHelper;
    public GameObject tutorialCavans;

    private int phase;
    private string[] messages;

    // Start is called before the first frame update
    void Start()
    {

        phase = 0;
        messages = new string[5];
        messages[0] = "Good evening, you're this club's newest bouncer and you need to decide who is allowed in, and who is not.";
        messages[1] = "To your right, you will see a ruleboard that changes at each stage.";
        messages[2] = "You'll need to check each patron against these rules along with their licence information.";
        messages[3] = "Once you have made your decision, simply press the button and continue.";
        messages[4] = "Make sure you thoroughly check all documentation provided, good luck.";

        isTutorialOn = false;
        tutorialHoldSpawn = true;

        // enable canvas and ui-helper
        uiHelper.SetActive(true);
        tutorialCavans.SetActive(true);

        // display the first slide
        UpdateSlide();
    }

    public void UpdateSlide()
    {
        content.text = messages[phase];
        AudioPlayer.PlayClip(AudioType.Speech, tutorialClips[phase]);

        if (phase == messages.Length - 1) 
            btnText.text = "Done";
    }

    public void Next()
    {
        phase++;

        if (phase == messages.Length)
        {
            Destroy(uiHelper);
            Destroy(tutorialCavans);
            Destroy(this);
        }
        else
        {
            if (phase == 2)
                tutorialHoldSpawn = false;

            UpdateSlide();
        }
    }
}
