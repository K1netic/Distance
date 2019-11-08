using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;

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
        yield return new WaitForSeconds(GameManager.timeBeforeRespawn);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y);
    }
}
