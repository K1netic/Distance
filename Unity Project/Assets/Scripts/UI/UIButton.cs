using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class UIButton : MonoBehaviour
{
    Camera cam;
    float fogTransitionDuration = 3.0f;

    // Fade to black
    [SerializeField] Texture2D fadeTexture;
    float fadeSpeed = 0.2f;
    int drawDepth = -1000;
    float alpha = 1.0f; 
    float fadeDir = -1;
    bool fadeToBlack = false;
    bool fogActivated = false;
    [FMODUnity.EventRef] public string inputsound;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        alpha = 0f;
    }

    void Update()
    {
        // Activate fog transition
        if (fogActivated)
        {
            D2FogsPE[] fogs = cam.GetComponents<D2FogsPE>();
            for(int i = 0; i < 2; i ++)
            {
                FogTransition(fogs[i], i);
            }
        }
    }

    // Launch game, fade screen to black and play a fog transition
    public void LaunchGame()
    {
        alpha = 0f;
        fadeToBlack = true;
        StartCoroutine(DisplayFog());
        StartCoroutine(CancelVibration (Vibrations.PlayVibration("TransitionToNextLevel")));
        FMODUnity.RuntimeManager.PlayOneShot(inputsound);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Resume()
    {
        FindObjectOfType<PauseMenu>().ClosePauseMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }

    void OnGUI()
    {
        // Fade to white
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0,0,1920f,1080f),fadeTexture);

        // Fade to black
        if (fadeToBlack)
        {
            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0,0,1920f,1080f),fadeTexture);
        }
    }

	public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}
    
    IEnumerator DisplayFog()
    {
        fogActivated = true;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = true;
        }
        yield return new WaitForSeconds(fogTransitionDuration);

        fogActivated = false;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = false;
        }
    }

    // variables needed to manage fog density over time
    float currentTime = 0f;
    float maxDensity;
    float animationTime;

    // Change fog density over time to display it smoothly
    // Two D2FogsPE scripts are used, each having one color
    void FogTransition(D2FogsPE fogScript, int index) 
    {
        // Total animation time
        animationTime = fogTransitionDuration * 2.0f;
        // Fog 1 max density
        if (index == 0) maxDensity = 5f;
        // Fog 2 max density
        else if (index == 1) maxDensity = 3f;

        // Lerp fog density from nearly none to its max for the whole animation time
        if (currentTime <= (animationTime))
        {
            currentTime += Time.deltaTime;
            fogScript.Density = Mathf.Lerp(0.2f, maxDensity, currentTime / animationTime);
        }

        // When the fog has reached its max density, load TUTORIAL level
        if (fogScript.Density == maxDensity)
        {
            SceneManager.LoadScene("TUTO");
        }
    }
}
