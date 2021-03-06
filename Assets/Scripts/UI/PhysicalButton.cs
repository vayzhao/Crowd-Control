using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class PhysicalButton : MonoBehaviour
{
    [Header("Button")]
    [Tooltip("Message to display on the button")]
    public string buttonText = "Button";
    [Tooltip("What happens when the button is pressed")]
    public UnityEvent function = new UnityEvent();
    [Tooltip("Whether or not to destroy the button when pressed")]
    public bool destroyOnPress = false;

    [Header("Spring")]
    [Tooltip("A position where will trigger button's function")]
    public float pressLength = 0.08f;
    [Tooltip("How strong the spring will pop up the button" +
        " when it is not being pressed by the player")]
    public float springForce = 1f;

    /// <summary>
    /// Necessary variables for triggering
    /// the button function
    /// </summary>
    private bool isPressed;
    private Vector3 originPos;
    private float triggerHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        // save origin position
        originPos = transform.position;

        // calculate button's trigger height
        triggerHeight = originPos.y - pressLength;

        // edit button's text
        GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
    }

    // Update is called once per frame
    void Update()
    {
        ButtonSpring();
        ButtonTrigger();
    }

    /// <summary>
    /// Method to determine whether or not the button is
    /// being pressed by the player
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Controllers"))
            isPressed = true;
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Controllers"))
            isPressed = false;
    }

    /// <summary>
    /// Method to release the button's height when it is
    /// not pressed by the player, also prevent the player
    /// to pull up the button
    /// </summary>
    void ButtonSpring()
    {
        // if it is not being pressed, reset its position
        if (!isPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, springForce * Time.deltaTime);
        }

        // force to reset the button position when the player
        // is trying to pull up the button
        if (transform.position.y > originPos.y)
        {
            transform.position = originPos;
        }
    }

    /// <summary>
    /// Method to trigger button's function, simply check 
    /// whether or not the button's current height is less
    /// or equal to the minimum height, if it is, call the
    /// button function
    /// </summary>
    void ButtonTrigger()
    {
        // check button's current height
        if (transform.position.y <= triggerHeight)
        {
            // save the pressing controller
            SavePressingController();

            // call the function
            function?.Invoke();

            // check to see if needs to destroy
            if (destroyOnPress)
            {
                // destroy the game object
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Method to save the pressing controller,
    /// the reason doing this is because we want
    /// the controller to vibrate when the player
    /// makes a wrong decision
    /// </summary>
    void SavePressingController()
    {
        // get position of left and right controller
        var lPos = Const.leftController.transform.position;
        var rPos = Const.rightController.transform.position;

        // get distance between those controller and the button
        var lDis = Vector3.Distance(transform.position, lPos);
        var rDis = Vector3.Distance(transform.position, rPos);

        // the controller that has the closest distance to the
        // button, is the pressing controller
        Const.pressingController = lDis <= rDis ? 
            Const.leftController : Const.rightController;
    }
    
    /// <summary>
    /// Method to reset the button to its original position
    /// and pressing status to be fause
    /// </summary>
    public void ResetButton(bool isHiding)
    {
        isPressed = false;
        transform.position = originPos;

        // hide the button if necessary
        if (isHiding)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
