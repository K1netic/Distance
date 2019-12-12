using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Projectile : MonoBehaviour {

	float acceleration;
	[HideInInspector] public float maxSpeed = 50;
	[SerializeField] float smoothTime = 0.3f;
	[SerializeField] float xVelocity = 10.0f;
	[SerializeField] float direction = 1f;
	Rigidbody2D rigid;
	[HideInInspector] public Transform respawnPoint;
	Animator playerAnimator;
	[HideInInspector] public GameObject projectileThrower;

	void Start () 
	{
		rigid = this.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate()
	{
		acceleration = Mathf.SmoothDamp(0, direction * maxSpeed, ref xVelocity, smoothTime);
		rigid.velocity = new Vector2(acceleration, 0);
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
