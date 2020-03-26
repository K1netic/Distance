using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{
    float currentTime = 0f;
    float maxDensity;
    float animationTime;
    Camera cam;
    GameObject player;
    float fogTransitionDuration = 2.0f;
    bool fogActivated = false;
    string sceneName = "";
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        // Position camera
        cam.transform.position = new Vector3(0,0,-10);
        // Position player at first spawnPoint
        player.transform.position = GameObject.Find("SpawnPoint_B1").transform.position;
        // Block player's movements
        PlayerMovement.lockMovement = true;
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        // Block skill use
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().LockSkillUse(skill);
        }
        // Fog transition
        StartCoroutine(DisplayFog());
    }

    void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (fogActivated)
        {
            D2FogsPE[] fogs = cam.GetComponents<D2FogsPE>();
            for(int i = 0; i < 2; i ++)
            {
                FogTransition(fogs[i], i);
            }
        }

        // Play music depending on scene name
        switch(sceneName)
        {
            case "TUTO":
                MusicManager.triggerTutoMusic = true;
                break;
            case "D":
                MusicManager.triggerDashMusic = true;
                break;
            case "J":
                MusicManager.triggerJumpMusic = true;
                break;
            default :
                break;
        }

    }

    IEnumerator DisplayFog()
    {
        // Activate fog
        fogActivated = true;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = true;
        }
        yield return new WaitForSeconds(fogTransitionDuration);
        // Deactivate fog once the transition is over
        fogActivated = false;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = false;
        }
        UnblockPlayerActions();

    }

    void UnblockPlayerActions()
    {
        PlayerMovement.lockMovement = false;
        player.GetComponent<Rigidbody2D>().isKinematic = false;

        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().UnlockSkillUse(skill);
        }
    }

    void FogTransition(D2FogsPE fogScript, int index) 
    {
        // Change fog density over time to make it appear smoothly
        animationTime = fogTransitionDuration * 2.0f;
        if (index == 0) maxDensity = 5f;
        else if (index == 1) maxDensity = 3f;

        if (currentTime <= animationTime)
        {
            currentTime += Time.deltaTime;
            fogScript.Density = Mathf.Lerp(maxDensity, 0.2f, currentTime / animationTime);
        }    
    }
}
