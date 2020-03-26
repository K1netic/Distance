using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Projectile : MonoBehaviour {

	// Speed management
	float xAcceleration;
	float yAcceleration;
	public float maxSpeed = 300f;
	float smoothTime = 0.3f;
	float xVelocity = 10.0f;
	float yVelocity = 10.0f;
	// Rotation
	float rotation;
	Rigidbody2D rigid;
	// Where the projectile should spawn from (managed in ProjectileThrower)
	[HideInInspector] public Transform respawnPoint;
	// Parent projectile thrower
	[HideInInspector] public GameObject projectileThrower;

	void Start () 
	{
		rigid = this.GetComponent<Rigidbody2D> ();
		// Determine rotation depending on orientation of the thrower
		switch(projectileThrower.GetComponent<ProjectileThrower>().orientation)
		{
			case "right":
				rotation = -90f;
				break;
			case "left":
				rotation = 90f;
				break;
			case "top":
				rotation = -180f;
				break;
			case "bottom":
				rotation = 180f;
				break;
			default:
				rotation = -90f;
				break;
		}
		transform.Rotate(new Vector3(0,0,rotation), Space.Self);
	}

	void FixedUpdate()
	{
		// Determine movement direction depending on thrower orientation
		switch(projectileThrower.GetComponent<ProjectileThrower>().orientation)
		{
			case "right":
				xAcceleration = Mathf.SmoothDamp(0, 1f * maxSpeed, ref xVelocity, smoothTime);
				yAcceleration = 0f;
				break;
			case "left":
				xAcceleration = Mathf.SmoothDamp(0, -1f * maxSpeed, ref xVelocity, smoothTime);
				yAcceleration = 0f;
				break;
			case "top":
				yAcceleration = Mathf.SmoothDamp(0, 1f * maxSpeed, ref yVelocity, smoothTime);
				xAcceleration = 0f;
				break;
			case "bottom":
				yAcceleration = Mathf.SmoothDamp(0, -1f * maxSpeed, ref yVelocity, smoothTime);
				xAcceleration = 0f;
				break;
			default:
				rotation = -90f;
				break;
		}

		// move
		rigid.velocity = new Vector2(xAcceleration, yAcceleration);
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		// Ignore potential collisions with canon in case it touches it on spawn
		if (coll.gameObject.tag == "Canon") 
			Physics2D.IgnoreCollision (this.GetComponent<Collider2D> (), coll.gameObject.GetComponent<Collider2D>(), true);
		// Destroy projectile if it touches anything
		else 
		{
			Destroy(this.gameObject, 0.05f);
		}
	}

	// Destroy projectile when it's out of the camera's view
	// CAUTION : This method also takes the editor's view into account
	// Meaning that items will only be destroyed when they are out of the editor's view
	// Shouldn't be a trouble in Build however
	void OnBecameInvisible()
	{
		Destroy (this.gameObject, 0.1f);
	}

}
