using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		// Stopping Item from moving and retrieving point of collision to use it as where the item should move towards
		if (other.gameObject.tag.Substring(0,4) == "Item")
			other.GetComponent<Proximity>().StopMoving(other.transform.position);
	}
}
