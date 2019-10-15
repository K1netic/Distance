using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

    [SerializeField]
    float dashForce = 25;
    [SerializeField]
    float lockMovementDuration = .25f;
    [SerializeField]
    float dashCooldown = 0.5f;

    Rigidbody2D rigid;
    bool dashAvailable = true;
    float localGravity;

    bool dashingOnGround = false;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        localGravity = rigid.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {

        //Player direction for dash
        var HorizontalInput = Input.GetAxisRaw("Horizontal");
        var VerticalInput = Input.GetAxisRaw("Vertical");

        // Récupération du dash
        if (Jump.isGrounded && !dashingOnGround) dashAvailable = true;

        if (dashAvailable)
        {
            #region Test direction
            //Dash dans la direction du joueur s'il dash sans bouger
            if (Input.GetButtonDown("Dash") && (HorizontalInput == 0 && VerticalInput == 0))
            {
				ApplyDash(new Vector2(PlayerMovement.playerDirection, 0));
            }

            if (Input.GetButtonDown("Dash") && Mathf.Abs(HorizontalInput) >= 0)
            {
                dashAvailable = false;
                dashingOnGround = true;
                Invoke("DashCooldown", dashCooldown);
            }

            //Dash dans la direction du joueur s'il essaye de dasher vers le haut
            if (Input.GetButtonDown("Dash") && (VerticalInput > 0.0f))
            {
                ApplyDash(new Vector2(PlayerMovement.playerDirection, 0));
            }

            //Dash droite
            if (Input.GetButtonDown("Dash") && (HorizontalInput > 0.0f && VerticalInput > -0.25f && VerticalInput < 0.25f))
            {
                ApplyDash(new Vector2(1, 0));
            }

            //Dash gauche
            if (Input.GetButtonDown("Dash") && (HorizontalInput < 0.0f && VerticalInput > -0.25f && VerticalInput < 0.25f))
            {
                ApplyDash(new Vector2(-1, 0));
            }

            //Dash bas
            if (Input.GetButtonDown("Dash") && (VerticalInput < 0.0f && HorizontalInput > -0.30f && HorizontalInput < 0.30f))
            {
                ApplyDash(new Vector2(0, -1));
            }

            //Dash bas-droite
            if (Input.GetButtonDown("Dash") && (HorizontalInput > 0.25f && VerticalInput > -1f && VerticalInput < -0.25f))
            {
                ApplyDash(new Vector2(1, -1));
            }

            //Dash bas-gauche
            if (Input.GetButtonDown("Dash") && (HorizontalInput < -0.25f && VerticalInput > -1f && VerticalInput < -0.25f))
            {
                ApplyDash(new Vector2(-1, -1));
            }
            #endregion
        }
    }

    //Lock player's movement, apply dash force then unlock the movement
    void ApplyDash(Vector2 direction)
    {
        PlayerMovement.lockMovement = true;
        dashAvailable = false;
        rigid.gravityScale = 0;
        rigid.velocity = Vector2.zero;
        rigid.velocity = direction * dashForce;
        Invoke("UnlockMovement", lockMovementDuration);
    }

    //Reset the gravity and the velocity, and let the player move again
    void UnlockMovement()
    {
        PlayerMovement.lockMovement = false;
		rigid.gravityScale = localGravity;
		rigid.velocity = Vector2.zero;
    }

    void DashCooldown()
    {
        dashAvailable = true;
        dashingOnGround = false;
    }

}
