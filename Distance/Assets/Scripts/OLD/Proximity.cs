using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour {

	[SerializeField] string itemType;
	GameObject player;
	Vector3 itemSourcePosition;
	Vector3 playerPosition;
	float distanceWithPlayer;
	float safeDistance = 3.0f;

	[SerializeField] float acceleration;
	[SerializeField] float deceleration;

	Rigidbody2D physics;

	Vector2 positionOfCollision;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerPosition = player.transform.position;
		// positionOfCollision = playerPosition;
		itemSourcePosition = this.transform.position;

		physics = GetComponent<Rigidbody2D>();
		physics.drag = deceleration;

		// Apply a starting force to give items a bit of movement
		Vector3 directionalForce = itemSourcePosition * acceleration;
		physics.AddForce(directionalForce * Time.deltaTime, ForceMode2D.Impulse);
	}

	// Update is called once per frame
	void Update () 
	{
		playerPosition = player.transform.position;
		distanceWithPlayer = Vector3.Distance(transform.position,playerPosition);

		// Getting Item closer to character
		if (Input.GetButton(itemType) && distanceWithPlayer > safeDistance)
		{
			Vector3 directionalForce = (playerPosition - transform.position) * acceleration;
			physics.AddForce(directionalForce * Time.deltaTime, ForceMode2D.Impulse);
		}

		else if(Input.GetButton(itemType) && distanceWithPlayer <= safeDistance)
		{
			Vector3 directionalForce = ((Vector3)positionOfCollision - transform.position) * acceleration;
			physics.AddForce(directionalForce * Time.deltaTime, ForceMode2D.Impulse);
		}

		// Getting Item farther from character (and back to its original position)
		else
		{
			Vector3 directionalForce = (itemSourcePosition - transform.position) * acceleration;
			physics.AddForce(directionalForce * Time.deltaTime, ForceMode2D.Impulse);
		}
	
	}

	public void StopMoving(Vector2 positionOfCollision)
	{
		positionOfCollision = new Vector2(positionOfCollision.x, positionOfCollision.y);
	}

}
