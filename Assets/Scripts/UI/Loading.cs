using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Text loadText;
    public GameObject loadTitle;
    public Canvas loadingCanvas;
    public Slider loadSlider;
    public Image loadingBackground;

    // Start is called before the first frame update
    void Start()
    {
        // start to load
        StartCoroutine("Load");        
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
            yield return new WaitForSeconds(Time.deltaTime * 5f);
        }

        // when finish loading, remove these UI components
        Destroy(loadText.gameObject);
        Destroy(loadTitle.gameObject);
        Destroy(loadSlider.gameObject);

        // start fading the background image 
        Color col = loadingBackground.color;
        while (col.a > 0f)
        {
            col.a -= Time.deltaTime;
            loadingBackground.color = col;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // remove the canvas
        Destroy(loadingCanvas.gameObject, 3f);
    }
}
