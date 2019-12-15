﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    float detectDistance = 0.5f;
    float detectRange = 1.5f;
    float topBottomHitsDistance = 1.5f;
    public LayerMask wallLayer;

    bool isOnLeftWall = false;
    bool isOnRightWall = false;
    Jump jumpScript;
    Rigidbody2D rigid;
    float wallJumpVerticalForce = 60f;
    float lockWallCheckDuration = 0.75f;
    bool lockLeftWallCheck = false;
    bool lockRightWallCheck = false;
    [FMODUnity.EventRef]
    public string inputsound;

    [SerializeField] GameObject JumpRing;

    // Start is called before the first frame update
    void Start()
    {
        jumpScript = gameObject.GetComponent<Jump>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

        //Saut vers la droite en étant collé à un mur à gauche
        if(Input.GetButtonDown("Jump") && isOnLeftWall)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, wallJumpVerticalForce);
            lockLeftWallCheck = true;
            Invoke("UnlockLeftWallCheck", lockWallCheckDuration);
            isOnLeftWall = false;
            PopParticle(-1.25f);
            //SOUND : WallJump
        }

        //Saut vers la gauche en étant collé à un mur à droite
        if(Input.GetButtonDown("Jump") && isOnRightWall)
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
		RaycastHit2D[] leftHits = Physics2D.RaycastAll(new Vector2(position.x - detectRange, position.y), leftDirection, detectDistance, wallLayer);
        RaycastHit2D[] topLeftHits = Physics2D.RaycastAll(new Vector2(position.x - detectRange, position.y + topBottomHitsDistance), leftDirection, detectDistance, wallLayer);
		RaycastHit2D[] bottomLeftHits = Physics2D.RaycastAll(new Vector2(position.x - detectRange, position.y - topBottomHitsDistance), leftDirection, detectDistance, wallLayer);

		for (int i = 0; i < leftHits.Length; i++)
		{
			RaycastHit2D leftHit = leftHits [i];
			if (leftHit.collider != null)
			{
				return true;
			}
		}

        for (int i = 0; i < topLeftHits.Length; i++)
		{
			RaycastHit2D topLeftHit = topLeftHits [i];
			if (topLeftHit.collider != null)
			{
				return true;
			}
		}

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
        Destroy(instantiated, instantiated.GetComponent<ParticleSystem>().main.duration + instantiated.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }

}