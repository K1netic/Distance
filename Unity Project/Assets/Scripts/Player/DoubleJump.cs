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

    [SerializeField] GameObject JumpRing;

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
        if (GroundCheck.isGrounded) airJumpAvailable = 1;

		// Saut II : le retour
		if (!GroundCheck.isGrounded && Input.GetButtonDown("Jump") && airJumpAvailable == 1)
		{
			float acceleration = Mathf.SmoothDamp(0, 1 * airJumpForce, ref yVelocity, 0.2f, airJumpForce);
			airJumpAvailable = 0;
            rigid.velocity = new Vector2(rigid.velocity.x, airJumpForce);
            PopParticle();
		}
    }

    void PopParticle()
    {
        GameObject instantiated = Instantiate(JumpRing,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 3f, 0), new Quaternion(0,0,0,0));
        instantiated.transform.Rotate(new Vector3(-90,0,0),Space.Self);
        Destroy(instantiated, instantiated.GetComponent<ParticleSystem>().main.duration + instantiated.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }
}
