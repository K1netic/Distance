using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(GameManager.timeBeforeRespawn);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y);
    }
}
