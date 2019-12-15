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

	[SerializeField] Transform respawnPoint;
	GameObject player;
	Animator playerAnimator;

	// Update is called once per frame
	void Start () 
	{
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
		Instantiate (FireBallGameObject, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0,0,0,0));
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
        //Vibrations
        StartCoroutine(CancelVibration (Vibrations.PlayVibration("Death")));
        //Arrêter le mouvement
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //Animations
        playerAnimator = player.gameObject.GetComponent<Animator>();
        playerAnimator.SetBool("dead", true);
        //Particules
        GameObject DeathParticles = player.gameObject.GetComponent<PlayerMovement>().DeathParticles;
        GameObject RespawnParticles = player.gameObject.GetComponent<PlayerMovement>().RespawnParticles;
        GameObject instantiatedDeathParticles = Instantiate(DeathParticles, player.transform.position, new Quaternion(0,0,0,0));
		//SOUND : Mort Joueur

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        GameObject instantiatedRespawnParticles = Instantiate(RespawnParticles, respawnPoint.transform.position, new Quaternion(0,0,0,0));
		//SOUND : Respawn Joueur

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        playerAnimator.SetBool("dead", false);
        //Déplacer le personnage au point de respawn
        player.transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        yield return new WaitForSeconds(0.2f);
        //Redonner la capacité de bouger
        player.GetComponent<Rigidbody2D>().isKinematic = false;

        yield return new WaitForSeconds(0.8f);
        //Détruire les particules instanciées
        Destroy(instantiatedDeathParticles);
        Destroy(instantiatedRespawnParticles);
    }
}
