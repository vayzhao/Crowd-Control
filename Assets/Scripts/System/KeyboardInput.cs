using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class KeyButton
{
    public KeyCode key;
    public Transform btn;

    public void Check()
    {
        // check to see if the button is active
        if (btn.gameObject.activeInHierarchy && Input.GetKey(key))
        {
            // get the spring force of the button
            var spring = btn.GetComponent<PhysicalButton>().springForce;

            // key pressing down the button
            btn.transform.position += Vector3.down * (spring * 2f) * Time.deltaTime;
        }
    }
}

public class KeyboardInput : MonoBehaviour
{
    public KeyButton[] keyButtons;

    void Update()
    {
        for (int i = 0; i < keyButtons.Length; i++)
        {
            keyButtons[i].Check();
        }
    }

}
