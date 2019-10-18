﻿using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionToNextLevel : MonoBehaviour
{
    string sceneToLoadName = "";
    int currentSceneIndex;

    Camera cam;
    GameObject player;
    float fogTransitionDuration = 3.0f;
    [SerializeField] Texture2D fadeTexture;
    float fadeSpeed = 0.2f;
    int drawDepth = -1000;
    
    float alpha = 1.0f; 
    float fadeDir = -1;

    float timeAtBegin;

    void Start()
    {
        currentSceneIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5,1));
        // Si le niveau en cours n'est pas le dernier
        if (currentSceneIndex + 1 < GameManager.numberOfLevels)
        {
            // On définit le prochain niveau dans le nom du niveau à charger
            sceneToLoadName = "Level" + (currentSceneIndex +1).ToString();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            timeAtBegin = Time.time;
            //Bloquer les mouvements du joueur 
            PlayerMovement.lockMovement = true;
            //Bloquer l'utilisation de compétences
            foreach(string skill in SkillsManagement.skills)
            {
                player.GetComponent<SkillsManagement>().LockSkillUse(skill);
            }
            //Charger l'animation de transition d'écran
            StartCoroutine(DisplayFog());
            //Fade To Black
            // LeanTween.alpha(fadeSprite.gameObject, 255, 2.0f);
            //Charger le prochain niveau
            SceneManager.LoadScene(sceneToLoadName);
        }
    }

    void OnGUI()
    {
		GUI.DrawTexture(new Rect(0,0,1920f,1080f),fadeTexture);
        
    }

    IEnumerator DisplayFog()
    {
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = true;
        }
        yield return new WaitForSeconds(fogTransitionDuration);
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
}