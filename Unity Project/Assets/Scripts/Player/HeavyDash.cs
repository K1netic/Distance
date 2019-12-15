using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class HeavyDash : MonoBehaviour
{
    float dashForce;
    float lockMovementDuration;
    float dashCooldown;

    Rigidbody2D rigid;
    bool dashAvailable = true;
    float localGravity;

    bool dashingOnGround = false;
    Dash dashScript;

    Animator playerAnimator;
    [SerializeField] GameObject HeavyDashParticles;
    [SerializeField] GameObject HeavyDashTrail;
    [SerializeField] GameObject HeavyDashShockwave;

    [FMODUnity.EventRef]
    public string inputsound;

    void Awake()
    {
        dashScript = gameObject.GetComponent<Dash>();
    }
    
    void OnEnable()
    {
        dashScript.lockNormalDash = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        dashForce = dashScript.dashForce;
        lockMovementDuration = dashScript.lockMovementDuration;
        dashCooldown = dashScript.dashCooldown;
        rigid = GetComponent<Rigidbody2D>();
        localGravity = rigid.gravityScale;
        playerAnimator = gameObject.GetComponent<Animator>();
    }
	
	void Update () {
        //Direction de l'input pour le dash
        var HorizontalInput = Input.GetAxisRaw("Horizontal");
        var VerticalInput = Input.GetAxisRaw("Vertical");

        // Récupération du dash
        if (GroundCheck.isGrounded && !dashingOnGround) dashAvailable = true;

        if (dashAvailable)
        {
            #region Test direction
            //Dash dans la direction du joueur s'il dash sans bouger
            if (Input.GetButtonDown("Dash") && (HorizontalInput == 0 && VerticalInput == 0))
            {
                ApplyDash(new Vector2(PlayerMovement.playerDirection, 0));
                PopParticleWithoutKnowingDirection();
            }

            // Récupération du dash dans le cas d'une utilisation au sol
            if (Input.GetButtonDown("Dash") && Mathf.Abs(HorizontalInput) >= 0 && GroundCheck.isGrounded)
            {
                dashAvailable = false;
                dashingOnGround = true;
                Invoke("DashCooldown", dashCooldown);
            }

            //Dash dans la direction du joueur s'il essaye de dasher vers le haut
            if (Input.GetButtonDown("Dash") && (VerticalInput > 0.0f))
            {
                ApplyDash(new Vector2(PlayerMovement.playerDirection, 0));
                PopParticleWithoutKnowingDirection();
            }

            //Dash droite
            if (Input.GetButtonDown("Dash") && (HorizontalInput > 0.0f && VerticalInput > -0.25f && VerticalInput < 0.25f))
            {
                ApplyDash(new Vector2(1, 0));
                PopParticle(HeavyDashParticles, 0.5f, 0, -90);
                PopParticle(HeavyDashTrail, 0.5f, 0, -90);
                PopParticle(HeavyDashShockwave, 4f, 0, -90);
            }

            //Dash gauche
            if (Input.GetButtonDown("Dash") && (HorizontalInput < 0.0f && VerticalInput > -0.25f && VerticalInput < 0.25f))
            {
                ApplyDash(new Vector2(-1, 0));
                PopParticle(HeavyDashParticles, -0.5f, 0, 90);
                PopParticle(HeavyDashTrail, -0.5f, 0, 90);
                PopParticle(HeavyDashShockwave, -4f, 0, 90);
            }

            //Dash bas
            if (Input.GetButtonDown("Dash") && (VerticalInput < 0.0f && HorizontalInput > -0.30f && HorizontalInput < 0.30f))
            {
                ApplyDash(new Vector2(0, -1));
                PopParticle(HeavyDashParticles, 0f, -90, -90);
                PopParticle(HeavyDashTrail, 0f, -90, -90);
                PopParticle(HeavyDashShockwave, 0f, -90, -90);
            }

            //Dash bas-droite
            if (Input.GetButtonDown("Dash") && (HorizontalInput > 0.25f && VerticalInput > -1f && VerticalInput < -0.25f))
            {
                ApplyDash(new Vector2(1, -1));
                PopParticle(HeavyDashParticles, 0f, -45, -90);
                PopParticle(HeavyDashTrail, 0f, -45, -90);
                PopParticle(HeavyDashShockwave, 0f, -45, -90);
            }

            //Dash bas-gauche
            if (Input.GetButtonDown("Dash") && (HorizontalInput < -0.25f && VerticalInput > -1f && VerticalInput < -0.25f))
            {
                ApplyDash(new Vector2(-1, -1));
                PopParticle(HeavyDashParticles, 0f, -135, -90);
                PopParticle(HeavyDashTrail, 0f, -135, -90);
                PopParticle(HeavyDashShockwave, 0f, -135, -90);
            }
            #endregion
        }
    }

    //Lock player's movement, apply dash force then unlock the movement
    void ApplyDash(Vector2 direction)
    {
        playerAnimator.SetBool("dashing", true);
        PlayerMovement.lockMovement = true;
        dashAvailable = false;
        rigid.gravityScale = 0;
        // rigid.velocity = Vector2.zero;
        rigid.velocity = direction * dashForce;
        StartCoroutine(CancelVibration (Vibrations.PlayVibration("HeavyDash")));
        Invoke("UnlockMovement", lockMovementDuration);
        FMODUnity.RuntimeManager.PlayOneShot(inputsound);
    }

    //Reset the gravity and the velocity, and let the player move again
    public void UnlockMovement()
    {
        playerAnimator.SetBool("dashing", false);
        PlayerMovement.lockMovement = false;
		rigid.gravityScale = localGravity;
		rigid.velocity = Vector2.zero;
    }

    void DashCooldown()
    {
        dashAvailable = true;
        dashingOnGround = false;
    }

    public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}

    void PopParticle(GameObject particleToPop, float xPosition, float xRotationAngle, float yRotationAngle)
    {
        GameObject instantiated = Instantiate(particleToPop,new Vector3(gameObject.transform.position.x + xPosition, gameObject.transform.position.y, 0), new Quaternion(0,0,0,0));
        instantiated.transform.Rotate(new Vector3(xRotationAngle,yRotationAngle,0),Space.Self);
        if (particleToPop != HeavyDashShockwave) instantiated.transform.parent = gameObject.transform;
        instantiated.transform.localScale = new Vector3(1,1,1);
        instantiated.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color ;//new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, gameObject.GetComponent<SpriteRenderer>().color.a);
        Destroy(instantiated, instantiated.GetComponent<ParticleSystem>().main.duration + instantiated.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }

    void PopParticleWithoutKnowingDirection()
    {
        if (PlayerMovement.playerDirection == 1) 
        {
            PopParticle(HeavyDashParticles, 0.5f, 0, -90);
            PopParticle(HeavyDashTrail, 0.5f, 0, -90);
            PopParticle(HeavyDashShockwave, 4f, 0, -90);
        }

        else if (PlayerMovement.playerDirection == -1)
        {
            PopParticle(HeavyDashParticles, -0.5f, 0, 90);
            PopParticle(HeavyDashTrail, -0.5f, 0, 90);
            PopParticle(HeavyDashShockwave, -4f, 0, 90);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Pushable" && playerAnimator.GetBool("dashing"))
        {
            other.gameObject.GetComponent<PushableObject>().Pushed();
        }
    }
}
