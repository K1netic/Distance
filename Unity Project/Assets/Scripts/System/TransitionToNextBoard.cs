using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;

public class TransitionToNextBoard : MonoBehaviour
{
    Camera cam;
    GameObject player;
    [SerializeField] Transform nextBoardSpawnPoint;
    float fogTransitionDuration = 1.5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
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
            //Charger l'animation de transition d'écran
            StartCoroutine(DisplayFog());
            //Déplacer la caméra sur le nouveau tableau
            cam.transform.position = new Vector3(cam.transform.position.x + 250, cam.transform.position.y, cam.transform.position.z);
            //Déplacer le joueur sur le nouveau tableau
            player.transform.position = new Vector3(nextBoardSpawnPoint.position.x, nextBoardSpawnPoint.position.y, 0);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
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
