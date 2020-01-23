using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class AutoKill : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    SpriteRenderer playerSprite;
    Transform spawnPoint;
    
    [FMODUnity.EventRef]
    public string inputsoundforRespawn;
    [FMODUnity.EventRef]
    public string inputsoundforDeath;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Autokill"))
            Kill();
    }

    void Kill()
    {
        if (transform.position.x < 125f)
            spawnPoint = GameObject.Find("SpawnPoint_B1").transform;
        else if (transform.position.x < 375f)
            spawnPoint = GameObject.Find("SpawnPoint_B2").transform;
        else if (transform.position.x < 625f)
            spawnPoint = GameObject.Find("SpawnPoint_B3").transform;
        else if (transform.position.x < 875f)
            spawnPoint = GameObject.Find("SpawnPoint_B4").transform;
        else if (transform.position.x < 1125f)
            spawnPoint = GameObject.Find("SpawnPoint_B5").transform;
        else if (transform.position.x < 1375f)
            spawnPoint = GameObject.Find("SpawnPoint_Transition").transform;
        StartCoroutine(RespawnPlayer(gameObject));
    }

    public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}

    IEnumerator RespawnPlayer(GameObject player)
    {
        //Arrêter le mouvement
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.isKinematic = true;

        //Particules
        GameObject DeathParticles = player.gameObject.GetComponent<PlayerMovement>().DeathParticles;
        DeathParticles.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = player.GetComponent<SpriteRenderer>().color;
        GameObject RespawnParticles = player.gameObject.GetComponent<PlayerMovement>().RespawnParticles;
        RespawnParticles.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = player.GetComponent<SpriteRenderer>().color;
        GameObject instantiatedDeathParticles = Instantiate(DeathParticles, player.transform.position, new Quaternion(0,0,0,0));
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundforRespawn);

        //Sprite
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0);

        //Vibrations
        StartCoroutine(CancelVibration (Vibrations.PlayVibration("Death")));
        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        GameObject instantiatedRespawnParticles = Instantiate(RespawnParticles, spawnPoint.transform.position, new Quaternion(0,0,0,0));
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundforDeath);

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        //Déplacer le personnage au point de respawn
        player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y);
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.isKinematic = true;

        yield return new WaitForSeconds(0.2f);
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);
        //Redonner la capacité de bouger
        playerRigidbody.isKinematic = false;

        yield return new WaitForSeconds(0.8f);
        //Détruire les particules instanciées
        Destroy(instantiatedDeathParticles);
        Destroy(instantiatedRespawnParticles);
    }
}
