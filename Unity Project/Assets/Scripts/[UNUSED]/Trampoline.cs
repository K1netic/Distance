using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Trampoline : MonoBehaviour {

	float knockBackTrampoline = 20f;
	PlayerMovement playerMovement;
	Rigidbody2D rigid;
    Animator anim;

	[SerializeField] bool isFlat = false;
	[SerializeField] bool vertical = false;

	void Start()
	{
        anim = GetComponent<Animator>();
		rigid = playerMovement.gameObject.GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// Contact point between player and trampoline
		Vector3 contact = collision.GetContact(0).point;
		Vector3 center = Vector3.zero;
		// Trampoline center
		if (!isFlat)
			center = this.transform.position;
		else if (isFlat)
			center = new Vector3(contact.x, transform.position.y, 0);
		if (vertical)
			center = new Vector3(transform.position.x, contact.y, 0);
		// Incident ray
		Vector3 incident = collision.transform.position - contact;
		// Normale
		Vector3 normale = contact - center;
		// Reflected ray
		Vector3 reflected = - Vector3.Reflect(incident, normale).normalized;
		Knockback(collision.gameObject, reflected);
		anim.SetTrigger("Touched");
	}

	// Apply a knockback on the player when they touch the trampoline
	void Knockback(GameObject player, Vector3 direction)
	{
        rigid.AddForce(direction * knockBackTrampoline, ForceMode2D.Impulse);
	}
}
