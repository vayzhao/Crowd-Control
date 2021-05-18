using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Controller")]
    [Tooltip("Should be LTouch or RTouch only")]
    public OVRInput.Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        // bind this controller to the blackboard
        if (controller == OVRInput.Controller.LTouch)
            Const.leftController = this;
        else if (controller == OVRInput.Controller.RTouch)
            Const.rightController = this;
    }

    /// <summary>
    /// Method to add vibration to the controller
    /// </summary>
    public void AddVibration(float frequency, float amplitude, float duration)
    {
        // vibrate the controller
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        // if the duration is less than 2 seconds, stop 
        // the vibration after the duration. if the duration
        // is longer than 2 seconds, the vibration will
        // stop automatically after 2 seconds
        if (duration < 2f)
            Invoke("RemoveVibration", duration);        
    }
    void RemoveVibration()
    {
        // stop the vibiration
        OVRInput.SetControllerVibration(0, 0, controller);
    }

}
