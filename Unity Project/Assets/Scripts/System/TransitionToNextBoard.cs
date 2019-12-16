using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;
using XInputDotNetPure;

public class TransitionToNextBoard : MonoBehaviour
{
    Camera cam;
    GameObject player;
    [SerializeField] Transform nextBoardSpawnPoint;
    [SerializeField] float newCameraPosition;
    float fogTransitionDuration = 0.75f;
    bool fogActivated = false;
    GameObject spawnLight;
    [FMODUnity.EventRef]
    public string inputsound;
    MusicManager musicManager;
    [SerializeField] bool zoneTransition = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnLight = player.transform.GetChild(0).gameObject;
        spawnLight.SetActive(false);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        musicManager = GameObject.FindObjectOfType<MusicManager>();
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
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Bloquer l'utilisation de compétences
            foreach(string skill in SkillsManagement.skills)
            {
                player.GetComponent<SkillsManagement>().LockSkillUse(skill);
            }
            //Charger l'animation de transition d'écran
            StartCoroutine(Transition());
            //Vibrations
		    StartCoroutine(CancelVibration (Vibrations.PlayVibration("TransitionToNextBoard")));
            //Fade music
            FMODUnity.RuntimeManager.PlayOneShot(inputsound);
            if (zoneTransition)
                musicManager.currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //Start Calculating distances between specters and player
            Specter[] specterScripts = FindObjectsOfType<Specter>();
            foreach(Specter script in specterScripts)
            {
                script.calculateDistance = true;
            }
        }
    }

    IEnumerator Transition()
    {
        //Déplacer la caméra sur le nouveau tableau
        cam.transform.position = new Vector3(newCameraPosition, cam.transform.position.y, cam.transform.position.z);
        //Afficher le brouillard
        fogActivated = true;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = true;
        }

        yield return new WaitForSeconds(fogTransitionDuration);
        cam.transform.position = new Vector3(newCameraPosition, cam.transform.position.y, cam.transform.position.z);

        fogActivated = false;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = false;
        }
        //Activer les mouvements du joueur
        PlayerMovement.lockMovement = false;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        //Activer l'utilisation des skills du joueur
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().UnlockSkillUse(skill);
        }
        //Déplacer le joueur sur le nouveau tableau
        player.transform.position = new Vector3(nextBoardSpawnPoint.position.x, nextBoardSpawnPoint.position.y, 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //FX de spawn
        spawnLight.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        spawnLight.SetActive(false);
        // cam.transform.position = new Vector3(newCameraPosition, cam.transform.position.y, cam.transform.position.z);
    }

	public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}


    float currentTime = 0f;
    float maxDensity;
    float animationTime;
    void FogTransition(D2FogsPE fogScript, int index) {
        animationTime = fogTransitionDuration * 2.0f;
        if (index == 0) maxDensity = 5f;
        else if (index == 1) maxDensity = 3f;

        if (currentTime <= (animationTime/2.0f))
        {
            currentTime += Time.deltaTime;
            fogScript.Density = Mathf.Lerp(0.2f, maxDensity, currentTime / animationTime);
        }
        else if (currentTime <= animationTime)
        {
            currentTime += Time.deltaTime;
            fogScript.Density = Mathf.Lerp(maxDensity, 0.2f, currentTime / animationTime);
        }
        else
        {
            fogScript.Density = maxDensity;
            currentTime = 0f;
        }
    }
}
