using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    // Distance from the player (on x) at which the raycast will be created
    float detectRange = 1.5f;
    // Distance from the raycast beginning point from which the walls can be detected
    float detectDistance = 0.5f;
    // Raycasts vertical offset at which they will be created
    float topBottomHitsDistance = 1.5f;
    public LayerMask wallLayer;
    bool isOnLeftWall = false;
    bool isOnRightWall = false;
    Jump jumpScript;
    Rigidbody2D rigid;
    float wallJumpVerticalForce = 60f;
    // Time before a wall jump can be executed again in the same direction
    float lockWallCheckDuration = 0.75f;
    bool lockLeftWallCheck = false;
    bool lockRightWallCheck = false;
    [FMODUnity.EventRef]
    public string inputsound;

    // Particles
    [SerializeField] GameObject JumpRing;

    void Start()
    {
        jumpScript = gameObject.GetComponent<Jump>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Left/Right wall detection management
        if (!lockLeftWallCheck) isOnLeftWall = checkIfOnLeftWall();
        
        if (!lockRightWallCheck) isOnRightWall = checkIfOnRightWall();

        if (isOnLeftWall)
        {
            lockRightWallCheck = false;
        }

        if (isOnRightWall)
        {
            lockLeftWallCheck = false;
        }

        // Jump to the right when being on a wall to the left
        if(Input.GetButtonDown("Jump") && isOnLeftWall && !GroundCheck.isGrounded)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, wallJumpVerticalForce);
            lockLeftWallCheck = true;
            Invoke("UnlockLeftWallCheck", lockWallCheckDuration);
            isOnLeftWall = false;
            PopParticle(-1.25f);
            FMODUnity.RuntimeManager.PlayOneShot(inputsound);
        }

        // Jump to the left when being on a wall to the right
        if(Input.GetButtonDown("Jump") && isOnRightWall && !GroundCheck.isGrounded)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, wallJumpVerticalForce);
            lockRightWallCheck = true;
            Invoke("UnlockRightWallCheck", lockWallCheckDuration);
            isOnRightWall = false;
            PopParticle(1.25f);
            FMODUnity.RuntimeManager.PlayOneShot(inputsound);
        }
    }

    bool checkIfOnLeftWall()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		Vector2 leftDirection = Vector2.left;
		Debug.DrawRay (new Vector2(position.x - detectRange, position.y), leftDirection * detectDistance, Color.red, 5f);
        // three raycasts are thrown to make sure a wall is well detected even if only the character's head or feet are near it
		RaycastHit2D[] leftHits = Physics2D.RaycastAll(new Vector2(position.x - detectRange, position.y), leftDirection, detectDistance, wallLayer);
        RaycastHit2D[] topLeftHits = Physics2D.RaycastAll(new Vector2(position.x - detectRange, position.y + topBottomHitsDistance), leftDirection, detectDistance, wallLayer);
		RaycastHit2D[] bottomLeftHits = Physics2D.RaycastAll(new Vector2(position.x - detectRange, position.y - topBottomHitsDistance), leftDirection, detectDistance, wallLayer);

        // Raycast thrown from the center of the character's y axis
		for (int i = 0; i < leftHits.Length; i++)
		{
			RaycastHit2D leftHit = leftHits [i];
			if (leftHit.collider != null)
			{
				return true;
			}
		}

        // Raycast thrown from the upper side of the character's y axis
        for (int i = 0; i < topLeftHits.Length; i++)
		{
			RaycastHit2D topLeftHit = topLeftHits [i];
			if (topLeftHit.collider != null)
			{
				return true;
			}
		}

        // Raycast thrown from the lower side of the character's y axis
        for (int i = 0; i < bottomLeftHits.Length; i++)
		{
			RaycastHit2D bottomLeftHit = bottomLeftHits [i];
			if (bottomLeftHit.collider != null)
			{
				return true;
			}
		}

        return false;
    }

    bool checkIfOnRightWall()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 rightDirection = Vector2.right;

		Debug.DrawRay (new Vector2(position.x + detectRange, position.y), rightDirection * detectDistance, Color.red, 5f);
        Debug.DrawRay (new Vector2(position.x + detectRange, position.y + topBottomHitsDistance), rightDirection * detectDistance, Color.red, 5f);
		Debug.DrawRay (new Vector2(position.x + detectRange, position.y - topBottomHitsDistance), rightDirection * detectDistance, Color.red, 5f);

		//Raycasts
		RaycastHit2D[] rightHits = Physics2D.RaycastAll(new Vector2(position.x + detectRange, position.y), rightDirection, detectDistance, wallLayer);
		RaycastHit2D[] topRightHits = Physics2D.RaycastAll(new Vector2(position.x + detectRange, position.y + topBottomHitsDistance), rightDirection, detectDistance, wallLayer);
		RaycastHit2D[] bottomRightHits = Physics2D.RaycastAll(new Vector2(position.x + detectRange, position.y - topBottomHitsDistance), rightDirection, detectDistance, wallLayer);

		for (int i = 0; i < rightHits.Length; i++)
		{
			RaycastHit2D rightHit = rightHits [i];
			if (rightHit.collider != null)
			{
				return true;
			}
		}

        for (int i = 0; i < topRightHits.Length; i++)
		{
			RaycastHit2D topRightHit = topRightHits [i];
			if (topRightHit.collider != null)
			{
				return true;
			}
		}
        
        for (int i = 0; i < bottomRightHits.Length; i++)
		{
			RaycastHit2D bottomRightHit = bottomRightHits [i];
			if (bottomRightHit.collider != null)
			{
				return true;
			}
		}

        return false;
    }

    void UnlockLeftWallCheck()
    {
        lockLeftWallCheck = false;
    }

    void UnlockRightWallCheck()
    {
        lockRightWallCheck = false;
    }

    void PopParticle(float xPosition)
    {
        GameObject instantiated = Instantiate(JumpRing,new Vector3(gameObject.transform.position.x + xPosition, gameObject.transform.position.y, 0), new Quaternion(0,0,0,0));
        instantiated.transform.Rotate(new Vector3(-180,-90,90),Space.Self);
        if (xPosition > 0)
        {
            instantiated.transform.GetChild(0).transform.Rotate(new Vector3(-180,0,0), Space.Self);
        }
        instantiated.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
        instantiated.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
        Destroy(instantiated, instantiated.GetComponent<ParticleSystem>().main.duration + instantiated.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }

}
