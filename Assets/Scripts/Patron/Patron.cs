using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatronFSM
{
    Spawn,
    Enter,
    Exit,
    Wait,
    Dance
}

public class Patron : MonoBehaviour
{
    private Animator animator;
    private PatronSpawner spawner;
    private float movementSpeed;
    private float rotationSpeed;

    public PatronFSM fsm;
    private Transform currentNode;

    // Method to setup a patron 
    public void Setup(PatronSpawner spawner, Transform path)
    {
        // bind the spawning script to this object
        this.spawner = spawner;

        // find animation conroller component
        animator = GetComponent<Animator>();

        // initialize variables
        fsm = PatronFSM.Spawn;
        movementSpeed = spawner.movementSpeed;
        rotationSpeed = spawner.rotationSpeed;

        // setup patron's DOB, name and suburb
        GetComponent<PatronInformation>().Setup();

        // start moving towards the player
        ChooseAPath(path);
    }

    /// <summary>
    /// Method for finding the start node of given 
    /// path, reset walking coroutine and animation
    /// </summary>
    void ChooseAPath(Transform path)
    {
        // stop the current walking coroutine
        StopCoroutine(Walking());

        // find the start node of the path
        currentNode = path.GetChild(0);

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
        while (currentNode)
        {
            // find current node's position
            var dest = currentNode.position;

            // rotate the patron
            var rot = Quaternion.LookRotation(dest - transform.position);
            var fix = Mathf.Min(rotationSpeed * Time.deltaTime, 1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, fix);

            // moving patron's position
            transform.position = Vector3.MoveTowards(transform.position,
                dest, movementSpeed * Time.deltaTime);

            // check to see if the patron has reached the destination
            if (Vector3.Distance(transform.position, dest) <= Const.DISTANCE_REACH) 
            {
                // check to see if the next node exists
                if (currentNode.childCount > 0)
                    currentNode = currentNode.GetChild(0);
                else
                    break;
            }

            // yielding
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // check to see what the patron is doing
        switch (fsm)
        {
            case PatronFSM.Spawn:
                Wait();
                break;
            case PatronFSM.Enter:
                Dance();
                break;
            case PatronFSM.Exit:
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Patron FSM Behaviour - Wait
    /// In this fsm, patorn is waiting for the player to 
    /// decide whether to accept or reject
    /// </summary>
    public void Wait()
    {
        // switch fsm to wait
        fsm = PatronFSM.Wait;

        // reset animation
        animator.Play("Idle");

        // force the patron to look at the player
        transform.eulerAngles = Vector3.up * 90f;

        // display decision button objects
        spawner.stageManager.DisplayDecisionComponents(true);

        // refresh licence data
        spawner.stageManager.licenceObject.GetComponent<Licence>().Refresh(this.GetComponent<PatronInformation>());
    }

    /// <summary>
    /// Patron FSM Behaviour - Dance
    /// In this fsm, patron will enter the dance fsm,
    /// It is called when the patron reaches the last
    /// node of enter path
    /// </summary>
    public void Dance()
    {
        // switch fsm to dance
        fsm = PatronFSM.Dance;

        // reset animation
        animator.Play("Jump");

        // into the dance floor
        spawner.stageManager.danceFloor.AddPatron(this.transform);
    }

    /// <summary>
    /// Accept the patron and the patron will walk into the club
    /// </summary>
    public void Accept()
    {
        // switch fsm to enter
        fsm = PatronFSM.Enter;

        // choose an entering path
        ChooseAPath(spawner.enterPath[Random.Range(0, spawner.enterPath.Length)]);
    }

    /// <summary>
    /// Reject the patron and the patron will choose a way to exit
    /// </summary>
    public void Reject()
    {
        // switch fsm to exit
        fsm = PatronFSM.Exit;

        // choose an exiting path
        ChooseAPath(spawner.exitPath[Random.Range(0, spawner.exitPath.Length)]);
    }
}
