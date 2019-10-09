using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour {

	[SerializeField] Sprite baseSprite;
	[SerializeField] Sprite crouchSprite;
	[SerializeField] Collider2D baseCollider;
	[SerializeField] Collider2D crouchCollider;

    SpriteRenderer currentSprite;

    private void Start()
    {
        currentSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () 
	{
		if (Input.GetAxis("Vertical") <= -0.1 && Input.GetAxis("Horizontal") == 0.0 && Jump.isGrounded)
		{
			baseCollider.enabled = false;
			crouchCollider.enabled = true;
            currentSprite.sprite = crouchSprite;
		}

		else
		{
			crouchCollider.enabled = false;
			baseCollider.enabled = true;
            currentSprite.sprite = baseSprite;
		}
	}
}
