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
    // Camera position after moving to next board
    [SerializeField] float newCameraPosition;
    // The final duration of the animation is two times this value
    float fogTransitionDuration = 0.75f;
    bool fogActivated = false;
    // Light that displays when the player appears
    GameObject spawnLight;
    [FMODUnity.EventRef] public string inputsound;
    MusicManager musicManager;
    [SerializeField] bool zoneTransition = false;
    bool moveCamera = false;
    bool cameraHasMoved = false;

    void Start()
    {
        // Setup required objects
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

        if (moveCamera && !cameraHasMoved)
        {
            // Move camere to next board
            cam.transform.position = new Vector3(newCameraPosition, cam.transform.position.y, cam.transform.position.z);
            cameraHasMoved = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Block player movement
            PlayerMovement.lockMovement = true;
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // Block skill use
            foreach(string skill in SkillsManagement.skills)
            {
                player.GetComponent<SkillsManagement>().LockSkillUse(skill);
            }
            // Load screen transition animation
            StartCoroutine(Transition());
            // Vibrations
		    StartCoroutine(CancelVibration (Vibrations.PlayVibration("TransitionToNextBoard")));
            // Play transition sound
            FMODUnity.RuntimeManager.PlayOneShot(inputsound);
            // Fade music
            if (zoneTransition)
                musicManager.currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //Start Calculating distances between specters and player (used in Specter to apply effects depending on how far the player is from the specters)
            Specter[] specterScripts = FindObjectsOfType<Specter>();
            foreach(Specter script in specterScripts)
            {
                script.calculateDistance = true;
            }
        }
    }

    IEnumerator Transition()
    {
        // Move player to new board
        player.transform.position = new Vector3(nextBoardSpawnPoint.position.x, nextBoardSpawnPoint.position.y, 0);
        // Set player velocity to 0
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        // Display fog
        fogActivated = true;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = true;
        }
        yield return new WaitForSeconds(fogTransitionDuration);

        // Hide fog
        fogActivated = false;
        foreach(D2FogsPE fogScript in cam.GetComponents<D2FogsPE>())
        {
            fogScript.enabled = false;
        }

        //Activate player movements
        PlayerMovement.lockMovement = false;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        //Activate player skills
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().UnlockSkillUse(skill);
        }
        //Spawn Light FX
        spawnLight.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        spawnLight.SetActive(false);
    }

	public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}

    // Variables needed to manage fog density over time
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

        // Lerp fog density from nearly none to its max for the first half of the animation
        if (currentTime < (animationTime/2.0f))
        {
            currentTime += Time.deltaTime;
            fogScript.Density = Mathf.Lerp(0.2f, maxDensity, currentTime / animationTime);
        }
        // Lerp fog density from its max to nearly none for the second half of the animation
        else if (currentTime <= animationTime)
        {
            moveCamera = true;
            currentTime += Time.deltaTime;
            fogScript.Density = Mathf.Lerp(maxDensity, 0.2f, currentTime / animationTime);
        }
        // Set density to max in between
        else
        {
            fogScript.Density = maxDensity;
            currentTime = 0f;
        }
    }
}
