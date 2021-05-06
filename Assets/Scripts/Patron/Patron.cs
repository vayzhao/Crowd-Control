using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    const float DISTANCE_REACH = 0.1f;

    public Transform[] spawnNodes;
    public Transform[] enterNodes;
    public Transform[] exitNodes;

    private Transform[] movingPath;

    public float movementSpeed = 20f;
    public int currentIndex;

    public bool done;

    public GameObject buttons;
    private DecisionBtn decisionBtn;

    public void Run()
    {
        done = false;
        
        decisionBtn = buttons.GetComponent<DecisionBtn>();
        movingPath = spawnNodes;

        StartCoroutine(WalkToCouter());
    }
    
    IEnumerator WalkToCouter()
    {
        var currentIndex = 0;
        var destination = movingPath[currentIndex].position;

        while (currentIndex < movingPath.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) <= DISTANCE_REACH)
            {
                currentIndex++;
                if (currentIndex < movingPath.Length)
                    destination = movingPath[currentIndex].position;

            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        buttons.SetActive(true);

        while (!decisionBtn.accept && !decisionBtn.reject)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        movingPath = decisionBtn.accept ? enterNodes : exitNodes;
        currentIndex = 0;
        destination = movingPath[currentIndex].position;

        done = true;
        decisionBtn.ResetState();
        buttons.SetActive(false);

        while (currentIndex < movingPath.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) <= DISTANCE_REACH)
            {
                currentIndex++;
                if (currentIndex < movingPath.Length)
                    destination = movingPath[currentIndex].position;

            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        this.gameObject.SetActive(false);
    }
}
