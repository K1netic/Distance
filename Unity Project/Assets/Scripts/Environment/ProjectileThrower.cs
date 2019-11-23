using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrower : MonoBehaviour {

	[SerializeField] GameObject FireBallGameObject;
	// Time to wait before each shoot
	[SerializeField] float startTime = 1.0f;
	[SerializeField] float cadency = 3.0f;

	// Use this to increase/decrease the boost applied on scale when a shoot is charged
	// A value of 1 will make the ammo fired's grow * the time of charge in seconds
	[SerializeField] float scale = 1.0f;
	[SerializeField] float projectileSpeed = 300f;

	// Update is called once per frame
	void Start () 
	{
		InvokeRepeating("Fire", startTime, cadency);
	}

	void Fire()
	{
		Debug.Log("fire");
		FireBallGameObject.GetComponent<Projectile>().maxSpeed = projectileSpeed;
		FireBallGameObject.transform.localScale = new Vector3(scale, scale, 0);
		Instantiate (FireBallGameObject, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0,0,0,0));
		// Tirer le projectile
	}
}
