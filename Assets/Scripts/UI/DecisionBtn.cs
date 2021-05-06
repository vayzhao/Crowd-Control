using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecisionBtn : MonoBehaviour
{
    public bool accept;
    public bool reject;

    public void Accept()
    {
        accept = true;
    }

    public void Reject()
    {
        reject = true;
    }

    public void ResetState()
    {
        accept = false;
        reject = false;
    }

    public void GoBackHomePage()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
