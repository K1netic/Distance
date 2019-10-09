using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

    [SerializeField]
    float dashForce = 25;
    [SerializeField]
    float lockMovementDuration = .25f;
    [SerializeField]
    float dashCooldown = 1;

    Rigidbody2D rigid;
    bool dashAvailable = true;
    float localGravity;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        localGravity = rigid.gravityScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //Player direction for dash
        var HorizontalInput = Input.GetAxisRaw("Horizontal");
        var VerticalInput = Input.GetAxisRaw("Vertical");

        if (dashAvailable)
        {
            #region Test direction
            //Dash in player's direction if he doesn't move
            if (Input.GetButtonDown("Dash") && (HorizontalInput == 0 && VerticalInput == 0))
            {
				ApplyDash(new Vector2(PlayerMovement.playerDirection, 0));
            }

            //Dash right
            if (Input.GetButtonDown("Dash") && (HorizontalInput > 0.0f && VerticalInput > -0.25f && VerticalInput < 0.25f))
            {
                ApplyDash(new Vector2(1, 0));
            }

            //Dash left
            if (Input.GetButtonDown("Dash") && (HorizontalInput < 0.0f && VerticalInput > -0.25f && VerticalInput < 0.25f))
            {
                ApplyDash(new Vector2(-1, 0));
            }

            //Dash top
            if (Input.GetButtonDown("Dash") && (VerticalInput > 0.0f && HorizontalInput > -0.30f && HorizontalInput < 0.30f))
            {
                ApplyDash(new Vector2(0, 1));
            }

            //Dash bot
            if (Input.GetButtonDown("Dash") && (VerticalInput < 0.0f && HorizontalInput > -0.30f && HorizontalInput < 0.30f))
            {
                ApplyDash(new Vector2(0, -1));
            }

            //Dash right up
            if (Input.GetButtonDown("Dash") && (HorizontalInput > 0.25f && VerticalInput < 1f && VerticalInput > 0.25f))
            {
                ApplyDash(new Vector2(1, 1));
            }

            //Dash right bot
            if (Input.GetButtonDown("Dash") && (HorizontalInput > 0.25f && VerticalInput > -1f && VerticalInput < -0.25f))
            {
                ApplyDash(new Vector2(1, -1));
            }

            //Dash left up
            if (Input.GetButtonDown("Dash") && (HorizontalInput < -0.25f && VerticalInput < 1f && VerticalInput > 0.25f))
            {
                ApplyDash(new Vector2(-1, 1));
            }

            //Dash left bot
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
        Invoke("RefreshDashCooldown", dashCooldown);
    }

    //Reset the gravity and the velocity, and let the player move again
    void UnlockMovement()
    {
        PlayerMovement.lockMovement = false;
		rigid.gravityScale = localGravity;
		rigid.velocity = Vector2.zero;
    }

    void RefreshDashCooldown()
    {
        dashAvailable = true;
    }
}
