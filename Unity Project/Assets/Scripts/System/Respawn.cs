using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    Animator playerAnimator;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(RespawnPlayer(other.gameObject));
        }
    }

    public void SendFeedbacks()
	{
		StartCoroutine(CancelVibration (Vibrations.PlayVibration("Death")));
	}

	public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}

    IEnumerator RespawnPlayer(GameObject player)
    {
        SendFeedbacks();
        playerAnimator = player.gameObject.GetComponent<Animator>();
        GameObject particles = player.gameObject.GetComponent<PlayerMovement>().DeathParticles;
        playerAnimator.SetBool("dead", true);
        Instantiate(particles, player.transform.position, new Quaternion(0,0,0,0));
        yield return new WaitForSeconds(GameManager.timeBeforeRespawn);
        playerAnimator.SetBool("dead", false);
        // Destroy(particles);
        // playerAnimator.SetBool("respawn", true);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y);
    }
}
