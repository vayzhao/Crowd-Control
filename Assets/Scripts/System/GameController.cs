using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Patron[] patrons;
    public int currentPatronIndex;

    public GameObject youWinPannel;

    // Start is called before the first frame update
    void Start()
    {
        patrons[currentPatronIndex].Run();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPatronIndex < patrons.Length)
        {
            if (patrons[currentPatronIndex].done)
            {
                //patrons[currentPatronIndex].gameObject.SetActive(false);
                currentPatronIndex++;

                if (currentPatronIndex == patrons.Length)
                {
                    youWinPannel.SetActive(true);
                }
                else
                {
                    patrons[currentPatronIndex].Run();
                }
                
            }
        }
    }
}
