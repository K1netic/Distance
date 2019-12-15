using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class successZone : MonoBehaviour
{
    [SerializeField] Specter specter;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            specter.testSucceed = true;
        }
    }
}
