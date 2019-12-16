using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    Animator playerAnimator;
    [FMODUnity.EventRef]
    public string inputsoundforRespawn;
    [FMODUnity.EventRef]
    public string inputsoundforDeath;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(RespawnPlayer(other.gameObject));
        }
    }

	public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
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
        DeathParticles.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = player.GetComponent<SpriteRenderer>().color;
        GameObject RespawnParticles = player.gameObject.GetComponent<PlayerMovement>().RespawnParticles;
        RespawnParticles.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = player.GetComponent<SpriteRenderer>().color;
        GameObject instantiatedDeathParticles = Instantiate(DeathParticles, player.transform.position, new Quaternion(0,0,0,0));
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundforRespawn);

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        GameObject instantiatedRespawnParticles = Instantiate(RespawnParticles, spawnPoint.transform.position, new Quaternion(0,0,0,0));
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundforDeath);

        yield return new WaitForSeconds(GameManager.timeBeforeRespawn/2.0f);

        playerAnimator.SetBool("dead", false);
        //Déplacer le personnage au point de respawn
        player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y);
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
