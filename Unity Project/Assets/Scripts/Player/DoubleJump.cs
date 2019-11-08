using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    // Saut dans l'air
	float airJumpForce;
	int airJumpAvailable = 1;
    float yVelocity = 0f;
    Rigidbody2D rigid;

    Jump jumpScript;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        jumpScript = gameObject.GetComponent<Jump>();
        airJumpForce = jumpScript.jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (Jump.isGrounded) airJumpAvailable = 1;

		// Saut II : le retour
		if (!Jump.isGrounded && Input.GetButtonDown("Jump") && airJumpAvailable == 1)
		{
			float acceleration = Mathf.SmoothDamp(0, 1 * airJumpForce, ref yVelocity, 0.2f, airJumpForce);
			airJumpAvailable = 0;
            rigid.velocity = new Vector2(rigid.velocity.x, airJumpForce);
		}
    }
}
