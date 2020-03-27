using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class SpawnManager : MonoBehaviour
{
    public static List<GameObject> spawnPoints = new List<GameObject>();
    Transform lastSpawnPoint;
    Animator playerAnimator;
    [FMODUnity.EventRef]
    public string inputsoundforRespawn;
    [FMODUnity.EventRef]
    public string inputsoundforDeath;
    GameObject Player;
    Rigidbody2D playerRigidbody;
    SpriteRenderer playerSprite;

    void Awake()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When meeting a new spawnPoint, register it as the last one in the list
        if (other.tag == "SpawnPoint" && !spawnPoints.Contains(other.gameObject))
        {
            spawnPoints.Add(other.gameObject);
        }
        
        // Respawn player when meeting with deadly obstacles
        else if (other.tag == "Deadly")
        {
            StartCoroutine(RespawnPlayer(gameObject));
        }
    }

    // Used for deadly objects that have a non-trigger collider
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Deadly")
        {
            StartCoroutine(RespawnPlayer(gameObject));
        }
    }

    IEnumerator RespawnPlayer(GameObject player)
    {

        GameManager.Instance.playerJustRespawn = true; 

        // Get the last spawnPoint registered
        lastSpawnPoint = spawnPoints[spawnPoints.Count - 1].transform;

        // Stop movement
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.isKinematic = true;
        PlayerMovement.lockMovement = true;

        // Generate death particles
        GameObject DeathParticles = gameObject.GetComponent<PlayerMovement>().DeathParticles;
        DeathParticles.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
        GameObject RespawnParticles = gameObject.GetComponent<PlayerMovement>().RespawnParticles;
        RespawnParticles.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
        GameObject instantiatedDeathParticles = Instantiate(DeathParticles, gameObject.transform.position, new Quaternion(0,0,0,0));
        // Death sound
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundforRespawn);

        // Sprite
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0);

        // Vibrations
        StartCoroutine(CancelVibration (Vibrations.PlayVibration("Death")));

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        // Generate respawn particles
        GameObject instantiatedRespawnParticles = Instantiate(RespawnParticles, lastSpawnPoint.transform.position, new Quaternion(0,0,0,0));
        // Respawn sound
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundforDeath);

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        // Move the character to the last spawnPoint
        player.transform.position = new Vector3(lastSpawnPoint.position.x, lastSpawnPoint.position.y);
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.isKinematic = true;

        yield return new WaitForSeconds(0.2f);
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);
        // Get movement back
        playerRigidbody.isKinematic = false;
        PlayerMovement.lockMovement = false;
        player.transform.rotation = new Quaternion(0,0,0,0);

        yield return new WaitForSeconds(0.8f);
        GameManager.Instance.playerJustRespawn = false; 
        // Destroy instantiated particles
        Destroy(instantiatedDeathParticles);
        Destroy(instantiatedRespawnParticles);
    }

    public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}
}
