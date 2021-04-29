using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Header("UI Components")]
    public Text loadText;
    public Slider loadSlider;
    public GameObject loadTitle;
    public GameObject loadingCanvas;    
    public Image[] loadingBackground;

    [Header("Player's components")]
    public Camera playerCamera;
    public GameObject teleportation;
    public LayerMask initalCulling;
    public LayerMask finalCulling;
    private float farClipPlane;

    // Start is called before the first frame update
    void Start()
    {
        // start to load
        StartCoroutine("Load");

        // hide everything except UI components
        farClipPlane = playerCamera.farClipPlane;
        playerCamera.cullingMask = initalCulling;
        playerCamera.farClipPlane = 2000f;
    }

    /// <summary>
    /// Method to load the scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator Load()
    {
        // start to load the progress bar
        while (loadSlider.value < 1f)
        {
            loadSlider.value += Time.deltaTime;
            loadText.text = (loadSlider.value * 100.05f).ToString("N0") + "%";
            yield return new WaitForSeconds(Time.deltaTime * 10f);
        }

        // when finish loading, remove these UI components
        Destroy(loadText.gameObject);
        Destroy(loadTitle.gameObject);
        Destroy(loadSlider.gameObject);
        
        
        // start fading the background image 
        Color col = loadingBackground[0].color;
        bool gameStarted = false;
        while (col.a > 0f)
        {
            col.a -= Time.deltaTime;
            for (int i = 0; i < loadingBackground.Length; i++)
                loadingBackground[i].color = col;

            if (!gameStarted && col.a < 0.4f)
                playerCamera.cullingMask = finalCulling;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        // reset player camera's far clip plane
        playerCamera.farClipPlane = farClipPlane;

        // enable player to move and teleport
        teleportation.SetActive(true);
        teleportation.GetComponentInParent<SimpleCapsuleWithStickMovement>().enabled = true;

        // remove the canvas and its object
        Destroy(loadingCanvas.gameObject, 3f);
    }
}
