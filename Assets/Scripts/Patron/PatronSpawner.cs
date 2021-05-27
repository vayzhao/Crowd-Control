using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
    [Header("Patron Setting")]
    [Tooltip("Movement speed for patrons")]
    public float movementSpeed = 20f;
    [Tooltip("Rotation speed for patrons")]
    public float rotationSpeed = 50f;

    [Header("Patron Prefabs")]
    [Tooltip("These are the patron objects that will\n" +
        "be spawned during the game")]
    public GameObject[] patronPrefabs;

    [Header("Spawn Setting")]
    [Tooltip("An empty gameobject that holds the patrons")]
    public Transform patronHolder;    

    [Header("Pathing")]
    [Tooltip("Path from spawn point to player")]
    public Transform[] spawnPath;
    [Tooltip("Path to enter the club")]
    public Transform[] enterPath;
    [Tooltip("Path to leave the club")]
    public Transform[] exitPath;

    /// <summary>
    /// Necessary variables for patron spawning
    /// </summary>
    [HideInInspector]
    public StageManager stageManager;
    private List<int> indexList;

    // Start is called before the first frame update
    void Start()
    {
        // find stage mananger component
        stageManager = this.GetComponent<StageManager>();

        // reset spawning indexs
        ResetIndex();
    }

    /// <summary>
    /// Method to initialize index list, index list contains
    /// indexs from 0 - patronPrefabs.length
    /// so when trying to spawn a patron, just randomly pop
    /// an index from the list and remove it after the patron 
    /// is spawned, this will make sure each patron we spawn 
    /// is different from the previous
    /// </summary>
    void ResetIndex()
    {
        indexList = new List<int>();
        for (int i = 0; i < patronPrefabs.Length; i++)
            indexList.Add(i);
    }
    int PopIndex()
    {
        // get a random position in the list
        var pop = indexList[Random.Range(0, indexList.Count)];

        // remove the index
        indexList.Remove(pop);

        // reset the index list when running out of elements
        if (indexList.Count == 0)
            ResetIndex();

        // return the index
        return pop;
    }

    /// <summary>
    /// Method to spawn a patron
    /// </summary>
    public Patron Spawn()
    {
        // get spawning index
        var index = PopIndex();

        // genurate spawn path
        var path = spawnPath[Random.Range(0, spawnPath.Length)];
        var startNode = path.GetChild(0);
        var nextNode = startNode.GetChild(0);
        var rotation = startNode.position - nextNode.position;

        // instantiate the game object
        var patron = Instantiate(patronPrefabs[index], patronHolder);
        patron.transform.position = startNode.position; 
        patron.transform.eulerAngles = rotation;
        
        // patrong setup
        var script = patron.AddComponent<Patron>();
        script.Setup(this, path);

        // disable spawning
        stageManager.readyToSpawn = false;

        // return the patron's script
        return script;
    }
}
