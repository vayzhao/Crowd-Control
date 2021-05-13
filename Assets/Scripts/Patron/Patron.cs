using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    private Animator animator;
    private PatronSpawner spawner;
    private Transform[] path;
    private int currentPathIndex;
    private float movementSpeed;
    private float rotationSpeed;
    private bool isInQueue;

    // Method to setup a patron 
    public void Setup(PatronSpawner spawner)
    {
        // bind the spawning script to this object
        this.spawner = spawner;

        // find animation conroller component
        animator = GetComponent<Animator>();

        // initialize variables
        isInQueue = true;
        movementSpeed = spawner.movementSpeed;
        rotationSpeed = spawner.rotationSpeed;

        // start moving towards the player
        ChooseAPath(spawner.spawnPath);
    }

    /// <summary>
    /// Method for a patron to choose a path 
    /// and start moving towards it
    /// </summary>
    /// <param name="chosenPath"></param>
    void ChooseAPath(Transform[] chosenPath)
    {
        // stop the current walking coroutine
        StopCoroutine(Walking());

        // reset path and path index
        path = chosenPath;
        currentPathIndex = 0;

        // start a new walking coroutine
        StartCoroutine(Walking());

        // play walking animation
        animator.Play("Walk");
    }

    /// <summary>
    /// Method for patrons to walk to a destination. If the 
    /// patron is in the queue, the decision buttons will 
    /// be displayed when the patron reaches the destination.
    /// If the patron is not in the queue 
    /// (which means he's entering or exiting)
    /// the patron will be destroyed when it reaches the destination 
    /// </summary>
    /// <returns></returns>
    IEnumerator Walking()
    {
        // keep walking when there is more node to go
        while (currentPathIndex < path.Length)
        {
            // moving patron's position
            transform.position = Vector3.MoveTowards(transform.position,
                path[currentPathIndex].position, movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(
                path[currentPathIndex].position - transform.position), 
                Mathf.Min(rotationSpeed * Time.deltaTime, 1f));

            // check to see if the patron reaches the turning node
            if (Vector3.Distance(transform.position, path[currentPathIndex].position) <= Const.DISTANCE_REACH)
            {
                currentPathIndex++;
            }

            // yielding
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // check to see if the patron is in queue
        if (isInQueue)
        {
            // if it is, set it to be false and resume
            // idle animation
            isInQueue = false;
            animator.Play("Idle");
            
            // display decision button objects
            spawner.stageManager.DisplayDecisionComponents(true);
        }
        else
        {
            // otherwise destroy the object
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Accept the patron and the patron will walk into the club
    /// </summary>
    public void Accept()
    {
        // choose the path
        ChooseAPath(spawner.enterPath);
    }

    /// <summary>
    /// Reject the patron and the patron will choose a way to exit
    /// </summary>
    public void Reject()
    {
        // choose the path 
        if (Random.Range(0f,1f) > 0.5f)
            ChooseAPath(spawner.exitPathA);
        else
            ChooseAPath(spawner.exitPathB);
    }
}
