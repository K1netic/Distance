using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Movement management
    bool moving = false;
    public static int playerDirection = 1;
    [SerializeField] float accelerationSmoothTime = 0.3f;
    [SerializeField] float topSpeed = 20f;
    [SerializeField] float decelerationSmoothTime = 0.1f;
    float xVelocity = 0.0f;
    Rigidbody2D rigid;

    // Sounds
    [FMODUnity.EventRef] public string inputSoundWood;
    [FMODUnity.EventRef] public string inputSoundGrass;

    // Used to block character's movement
	public static bool lockMovement;
    
    Animator playerAnimator;

    // Particles
    public GameObject DeathParticles;
    public GameObject RespawnParticles;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerAnimator = gameObject.GetComponent<Animator>();
        // Play footsteep sound every 0.4s
        InvokeRepeating("CallFootsteps", 0, 0.4f);
    }

    void CallFootsteps()
    {
        if (moving)
        {
            if (GroundCheck.isGrounded)
            {
                FMODUnity.RuntimeManager.PlayOneShot(inputSoundWood);
            }
            else if (GroundCheck.isOnGrass)
            {
                FMODUnity.RuntimeManager.PlayOneShot(inputSoundGrass);
            }
            
        }
    }

    void FixedUpdate()
    {
        if (!lockMovement)
        {
            // Manage movement direction
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                playerDirection = 1;
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                playerDirection = -1;
            }

            // Start a movement smoothly if an horizontal input is registered (ACCELERATION)
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                playerAnimator.SetBool("running", true);
                float acceleration = Mathf.SmoothDamp(0, playerDirection * topSpeed, ref xVelocity, accelerationSmoothTime, topSpeed);
                rigid.velocity = new Vector2(acceleration, rigid.velocity.y);
                moving = true;
            }

            // Stop movement smoothly when the input is released (DECELERATION) 
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                playerAnimator.SetBool("running", false);
                float deceleration = Mathf.SmoothDamp(rigid.velocity.x, 0, ref xVelocity, decelerationSmoothTime, topSpeed);
                rigid.velocity = new Vector2(deceleration, rigid.velocity.y);
                moving = false;
            }
        }
    }

    // Make character move with a moving platform if they are on it
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "MovingPlatform") transform.parent = coll.transform;
	}

    // Stop character from moving accordingly to a moving platform's movement when they leave it
	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "MovingPlatform") transform.parent = null;
	}
}