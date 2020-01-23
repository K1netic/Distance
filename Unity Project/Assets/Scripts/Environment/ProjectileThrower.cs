using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ProjectileThrower : MonoBehaviour {

	[SerializeField] GameObject FireBallGameObject;
	// Time to wait before each shoot
	[SerializeField] float startTime = 1.0f;
	[SerializeField] float cadency = 3.0f;

	// Use this to increase/decrease the boost applied on scale when a shoot is charged
	// A value of 1 will make the ammo fired's grow * the time of charge in seconds
	[SerializeField] float scale = 1.0f;
	[SerializeField] float projectileSpeed = 300f;
	[SerializeField] public string orientation = "right";
	float xOffset = 0f;
	float yOffset = 0f;

	[SerializeField] Transform respawnPoint;
	GameObject player;
	Animator playerAnimator;
    Rigidbody2D playerRigidbody;
    SpriteRenderer playerSprite;
	    [FMODUnity.EventRef]
    public string inputsoundforRespawn;
    [FMODUnity.EventRef]
    public string inputsoundforDeath;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void Start () 
	{
		// Faire varier la position et la rotation des 
		player = GameObject.FindGameObjectWithTag("Player");
		InvokeRepeating("Fire", startTime, cadency);
	}

	void Fire()
	{
		FireBallGameObject.GetComponent<Projectile>().maxSpeed = projectileSpeed;
		FireBallGameObject.transform.localScale = new Vector3(scale, scale, 0);
		FireBallGameObject.GetComponent<Projectile>().respawnPoint = respawnPoint;
		FireBallGameObject.GetComponent<Projectile>().projectileThrower = gameObject;
		// Tirer le projectile
		switch(orientation)
		{
			case "right":
				xOffset = 4.25f;
				break;
			case "left":
				xOffset = -4.25f;
				break;
			case "top": 
				yOffset = 4.25f;
				break;
			case "bottom":
				yOffset = -4.25f;
				break;
			default:
				xOffset = 0f;
				yOffset = 0f;
				break;
		}
		Instantiate (FireBallGameObject, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, -1), new Quaternion(0,0,0,0));
	}

	public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}

	public void RespawnFromProjectile()
	{
		StartCoroutine(RespawnPlayer(player));
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

        GameObject instantiatedRespawnParticles = Instantiate(RespawnParticles, respawnPoint.transform.position, new Quaternion(0,0,0,0));
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundforDeath);

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        //Déplacer le personnage au point de respawn
        player.transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y);
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
