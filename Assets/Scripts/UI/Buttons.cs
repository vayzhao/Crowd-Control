using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    const string TAG_FIRSTLAYER = "UI_FirstLayer";
    const string TAG_SECONDLAYER = "UI_SecondLayer";

    public GameObject firstLayer;
    public GameObject secondLayer;

    /// <summary>
    /// Actual activate / deactivate a panel object
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="isOn"></param>
    public void ManipulatePanel(GameObject panel, bool isOn)
    {
        // show / hide the first layer panels / second layer panels
        if (panel.CompareTag(TAG_FIRSTLAYER))
        {
            firstLayer.SetActive(isOn);
            secondLayer.SetActive(!isOn);
        }
        else if (panel.CompareTag(TAG_SECONDLAYER))
        {
            firstLayer.SetActive(!isOn);
            secondLayer.SetActive(isOn);
        }

        // show / hide the panel 
        panel.SetActive(isOn);
    }

    /// <summary>
    /// Show a specific panel object
    /// </summary>
    /// <param name="panel">the manipulated panel object</param>
    public void ShowPanel(GameObject panel)
    {
        ManipulatePanel(panel, true);
    }

    /// <summary>
    /// Hide a specific panel object
    /// </summary>
    /// <param name="panel">the manipulated panel object</param>
    public void HidePanel(GameObject panel)
    {
        ManipulatePanel(panel, false);
    }

    /// <summary>
    /// It's called when a close button is clicked
    /// </summary>
    public void Close()
    {
        // get the triggered button
        Transform current = EventSystem.current.currentSelectedGameObject.transform;

        // declear a variable to store parent panel object
        GameObject parentPanel = null;

        // keep looking for parent object until there is no parent or found
        // a parent with layer tag
        while (current.parent != null && parentPanel == null)
        {
            if (current.parent.CompareTag(TAG_FIRSTLAYER) || current.parent.CompareTag(TAG_SECONDLAYER))
                parentPanel = current.parent.gameObject;
            else
                current = current.parent;
        }

        // hide the parent panel object
        if (parentPanel!=null)
            HidePanel(parentPanel);
    }

    /// <summary>
    /// Start a new game
    /// </summary>
    public void StartNewGame(bool hasTutorial)
    {
        Tutorial.isTutorialOn = hasTutorial;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
