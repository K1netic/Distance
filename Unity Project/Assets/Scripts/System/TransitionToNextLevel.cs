using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class TransitionToNextLevel : MonoBehaviour
{
    [SerializeField] string sceneToLoadName = "";
    int currentSceneIndex;
    Camera cam;
    GameObject player;
    float fogTransitionDuration = 3.0f;

    // Fade to black
    [SerializeField] Texture2D fadeTexture;
    float fadeSpeed = 0.2f;
    int drawDepth = -1000;
    float alpha = 1.0f; 
    float fadeDir = -1;

    bool fadeToBlack = false;
    bool fogActivated = false;

    void Start()
    {
        // currentSceneIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5,1));
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        alpha = 0f;
        // fadeToBlack = true;

    }

    void Update()
    {
        if (fogActivated)
        {
            D2FogsPE[] fogs = cam.GetComponents<D2FogsPE>();
            for(int i = 0; i < 2; i ++)
            {
                FogTransition(fogs[i], i);
            }
        }        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Bloquer les mouvements du joueur 
            PlayerMovement.lockMovement = true;
            //Bloquer l'utilisation de compétences
            foreach(string skill in SkillsManagement.skills)
            {
                player.GetComponent<SkillsManagement>().LockSkillUse(skill);
            }
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            alpha = 0f;
            fadeToBlack = true;
            //Charger l'animation de transition d'écran
            StartCoroutine(DisplayFog());
            StartCoroutine(CancelVibration (Vibrations.PlayVibration("TransitionToNextLevel")));
        }
    }

    void OnGUI()
    {
        // Fade to white
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0,0,1920f,1080f),fadeTexture);

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
        //Activer les mouvements du joueur
        PlayerMovement.lockMovement = false;
        //Activer l'utilisation des skills du joueur
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().UnlockSkillUse(skill);
        }
    }

    float currentTime = 0f;
    float maxDensity;
    float animationTime;
    void FogTransition(D2FogsPE fogScript, int index) {
        animationTime = fogTransitionDuration * 2.0f;
        if (index == 0) maxDensity = 5f;
        else if (index == 1) maxDensity = 3f;

        if (currentTime <= (animationTime))
        {
            currentTime += Time.deltaTime;
            fogScript.Density = Mathf.Lerp(0.2f, maxDensity, currentTime / animationTime);
        }
        else SceneManager.LoadScene(sceneToLoadName);

        // else if (currentTime <= animationTime)
        // {
        //     currentTime += Time.deltaTime;
        //     fogScript.Density = Mathf.Lerp(maxDensity, 0.2f, currentTime / animationTime);
        // }
        // else
        // {
        //     fogScript.Density = maxDensity;
        //     currentTime = 0f;
        // }
    }
}
