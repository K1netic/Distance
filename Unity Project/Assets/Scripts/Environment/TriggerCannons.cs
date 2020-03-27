using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCannons : MonoBehaviour
{

    [SerializeField] GameObject[] Canons;
    [SerializeField] GameObject[] PushableBarriers;

    bool activateCanons = false;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(GameObject Canon in Canons)
        {
            Canon.GetComponent<ProjectileThrower>().enabled = false;
        }
    }

    void Update()
    {
        if (activateCanons)
        {
            if (GameManager.Instance.playerJustRespawn)
            {
                // Destroy all projectiles
                Projectile[] projectiles = GameObject.FindObjectsOfType<Projectile>();
                foreach(Projectile projectile in projectiles)
                {
                    Debug.Log("projectile");
                    Destroy(projectile.gameObject);
                }

                // Block canons until player is done respawning
                foreach(GameObject Canon in Canons)
                {
                    Canon.GetComponent<ProjectileThrower>().enabled = false;
                }

                // Reset barriers to neutral position if they were moved
                foreach(GameObject Barrier in PushableBarriers)
                {
                    if (Barrier.GetComponent<PushableObject>().fallRight && !Barrier.GetComponent<Rigidbody2D>().isKinematic)
                        Barrier.GetComponent<Rigidbody2D>().velocity = new Vector2(-50,50);
                    else if (!Barrier.GetComponent<PushableObject>().fallRight && !Barrier.GetComponent<Rigidbody2D>().isKinematic)
                        Barrier.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,50f);
                }
            }

            else   
            {
                // Reset canons when player is done respawning
                foreach(GameObject Canon in Canons)
                {
                    Canon.GetComponent<ProjectileThrower>().enabled = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            activateCanons = true;
        }
    }
}
