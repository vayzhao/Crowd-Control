using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodadPatron : MonoBehaviour
{
    public string animateName;
    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAnimator.Play(animateName);
    }
}
