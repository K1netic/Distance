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

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		// Fire projectiles repeatedly
		InvokeRepeating("Fire", startTime, cadency);
	}

	void Fire()
	{
		// Setup projectile properties
		FireBallGameObject.GetComponent<Projectile>().maxSpeed = projectileSpeed;
		FireBallGameObject.transform.localScale = new Vector3(scale, scale, 0);
		FireBallGameObject.GetComponent<Projectile>().respawnPoint = respawnPoint;
		FireBallGameObject.GetComponent<Projectile>().projectileThrower = gameObject;
		// Fire projectile on edge of the canon
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
		// Instantiate a projectile
		Instantiate (FireBallGameObject, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, -1), new Quaternion(0,0,0,0));
	}

	// Stop all vibrations
	public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}
}
