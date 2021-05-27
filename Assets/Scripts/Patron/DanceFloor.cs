using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloor : MonoBehaviour
{
    public float danceFloorSize = 2f;
    public float rotateSpeed = 50f;
    public List<Transform> patronList;

    // Start is called before the first frame update
    void Start()
    {
        patronList = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    /// <summary>
    /// Method to rotate the dance floor object
    /// </summary>
    void Rotate()
    {
        // get dance floor's current euluer angles
        // then increase its Y axis value
        var rot = transform.eulerAngles;
        rot.y += rotateSpeed * Time.deltaTime;

        // update euler angles
        transform.eulerAngles = rot;
    }

    /// <summary>
    /// Method to add new patrons into the dance floor
    /// </summary>
    /// <param name="patron"></param>
    public void AddPatron(Transform patron)
    {
        // add this patron to the patron list
        // and parent it
        patronList.Add(patron);
        patron.parent = this.transform;

        // if the dance floor has 1 patron only, move
        // the patron to the center of the dance floor
        if (patronList.Count == 1)
        {
            patron.localPosition = Vector3.zero;
            return;
        }

        // if the dance floor has more than 1 patron
        // re-calculate polar positions for all patrons
        var gap = 360f / (float)patronList.Count;
        for (int i = 0; i < patronList.Count; i++)
        {
            var angle = gap * i;
            var x = danceFloorSize * Mathf.Cos(angle * Mathf.Deg2Rad);
            var z = danceFloorSize * Mathf.Sin(angle * Mathf.Deg2Rad);
            patronList[i].localPosition = new Vector3(x, 0f, z);
            patronList[i].LookAt(transform);
        }
    }

    /// <summary>
    /// Method to clean up dance floor
    /// </summary>
    public void Clear()
    {
        // look through patron list and remove all of them
        var count = patronList.Count;
        for (int i = 0; i < count; i++)
        {
            var patron = patronList[0];
            patronList.Remove(patron);
            Destroy(patron.gameObject);
        }
    }

}
