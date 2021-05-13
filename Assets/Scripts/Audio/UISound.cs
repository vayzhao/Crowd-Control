using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour
{
    [Header("Audio Source")]
    [Tooltip("Sorce that plays the UI button sounds")]
    public AudioSource player;

    [Header("Audio Clips")]
    [Tooltip("Audio clip to play when the pointer " +
        "hover on the button")]
    public AudioClip hoverSfx;
    [Tooltip("Audio clip to play when the pointer " +
        "click on the button")]
    public AudioClip clickSfx;

    // Start is called before the first frame update
    void Start()
    {
        // apply UI button sound to all buttons
        AddEventsToAllButtons();
    }

    /// <summary>
    /// Method to apply button sounds to 
    /// all the buttons in the scene
    /// </summary>
    void AddEventsToAllButtons()
    {
        // first, find all game object that has
        // button component attached onto it
        var btns = Resources.FindObjectsOfTypeAll<Button>();

        // create hover event
        EventTrigger.Entry hover = new EventTrigger.Entry();
        hover.eventID = EventTriggerType.PointerEnter;
        hover.callback.AddListener((data) => OnPointerEnter((PointerEventData)data));

        // create click event
        EventTrigger.Entry click = new EventTrigger.Entry();
        click.eventID = EventTriggerType.PointerClick;
        click.callback.AddListener((data) => OnPotinerClick((PointerEventData)data));

        // run through each button
        for (int i = 0; i < btns.Length; i++)
        {
            // find event trigger from the button
            var trg = btns[i].gameObject.GetComponent<EventTrigger>();

            // if the button does not have event trigger, add one onto it
            if (!trg)
                trg = btns[i].gameObject.AddComponent<EventTrigger>();

            // clear and register events for the trigger
            trg.triggers.Clear();
            trg.triggers.Add(hover);
            trg.triggers.Add(click);
        }
    }

    /// <summary>
    /// Methods to play ui button sounds
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerEnter(PointerEventData data)
    {
        player.PlayOneShot(hoverSfx);
    }
    public void OnPotinerClick(PointerEventData data)
    {
        player.PlayOneShot(clickSfx);
    }




}
