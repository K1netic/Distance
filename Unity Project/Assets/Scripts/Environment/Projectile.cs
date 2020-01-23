using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Projectile : MonoBehaviour {

	float xAcceleration;
	float yAcceleration;
	public float maxSpeed = 300f;
	float smoothTime = 0.3f;
	float xVelocity = 10.0f;
	float yVelocity = 10.0f;
	float rotation;
	Rigidbody2D rigid;
	[HideInInspector] public Transform respawnPoint;
	Animator playerAnimator;
	[HideInInspector] public GameObject projectileThrower;

	void Start () 
	{
		rigid = this.GetComponent<Rigidbody2D> ();
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

		rigid.velocity = new Vector2(xAcceleration, yAcceleration);
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Canon") 
			Physics2D.IgnoreCollision (this.GetComponent<Collider2D> (), coll.gameObject.GetComponent<Collider2D>(), true);
		else if (coll.gameObject.tag == "Player")
		{
			Destroy(this.gameObject);
			projectileThrower.GetComponent<ProjectileThrower>().RespawnFromProjectile();
		}
		else 
		{
			Destroy(this.gameObject, 0.05f);
		}
		// else 
		// 	Physics2D.IgnoreCollision (this.GetComponent<Collider2D> (), coll.gameObject.GetComponent<Collider2D>(), false);
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
