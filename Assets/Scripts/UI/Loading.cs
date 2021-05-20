using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Header("Load Setting")]
    public bool showLoading;

    [Header("UI Components")]
    public Text loadText;
    public Slider loadSlider;
    public GameObject loadTitle;
    public GameObject loadingCanvas;    
    public Image[] loadingBackground;

    [Header("Player's components")]
    public Camera playerCamera;
    public LayerMask initalCulling;
    public LayerMask finalCulling;
    private float farClipPlane;

    [Header("Core Mechanic")]
    public GameObject gameManager;
    public GameObject inGameCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // check to see if showing loading
        if (showLoading)
        {
            // activate loading canvas
            loadingCanvas.SetActive(true);

            // start to load
            StartCoroutine("Load");

            // hide everything except UI components
            farClipPlane = playerCamera.farClipPlane;
            playerCamera.cullingMask = initalCulling;
            playerCamera.farClipPlane = 2000f;
        }
        else
        {
            // destroy loading UI components
            Destroy(loadingCanvas);

            // start the game
            GameStart();
        }        
    }

    /// <summary>
    /// Method to load the scene
    /// </summary>
    /// <returns></returns>
    IEnumerator Load()
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

        // remove the canvas and its object
        Destroy(loadingCanvas.gameObject, 3f);

        // start the game
        GameStart();
    }

    /// <summary>
    /// Method to start up core game script after
    /// loading is done
    /// </summary>
    void GameStart()
    {
        // set up date data
        Const.SetupDate();

        // start up game manager and in game canvas
        gameManager.SetActive(true);
        inGameCanvas.SetActive(true);
    }
}
