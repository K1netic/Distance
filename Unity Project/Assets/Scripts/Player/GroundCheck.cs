using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class GroundCheck : MonoBehaviour
{
    Animator playerAnimator;
	public static bool floorTest = false;
    public static bool isGrounded;
    float groundDetectDistance = 0.01f;
	float groundDetectRange = 0.8f;
	public Transform feetPos;
	public LayerMask groundLayer;


    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Test si le personnage est au sol
        isGrounded = checkIfGrounded();

        // Falling
		if (!isGrounded) 
		{
			playerAnimator.SetBool("falling", true);
			StartCoroutine(RefreshFloorTest());
		}

		// Check if falling on floor
		if (checkIfGrounded() && !floorTest)
		{
			playerAnimator.SetBool("jumping", false);
			playerAnimator.SetBool("falling", false);
        	StartCoroutine(CancelVibration (Vibrations.PlayVibration("FallingOnFloor")));
			//SOUND : Recovery Sol
			floorTest = true;
		}
        
    }
    
    bool checkIfGrounded() 
	{
		Vector2 position = new Vector2(feetPos.position.x, feetPos.position.y);
		Vector2 direction = Vector2.down;

		Debug.DrawRay (new Vector2(position.x - groundDetectRange, position.y), direction * groundDetectDistance, Color.cyan);
		Debug.DrawRay (new Vector2(position.x + groundDetectRange, position.y), direction * groundDetectDistance, Color.cyan);
		//Raycasts
		RaycastHit2D[] leftHits = Physics2D.RaycastAll(new Vector2(position.x - groundDetectRange, position.y), direction, groundDetectDistance, groundLayer);
		RaycastHit2D[] rightHits = Physics2D.RaycastAll(new Vector2(position.x + groundDetectRange, position.y), direction, groundDetectDistance, groundLayer);

		for (int i = 0; i < leftHits.Length; i++)
		{
			RaycastHit2D leftHit = leftHits [i];
			if (leftHit.collider != null)
			{
				return true;
			}
		}

		for (int i = 0; i < rightHits.Length; i++)
		{
			RaycastHit2D rightHit = rightHits [i];
			if (rightHit.collider != null)
			{
				return true;
			}
		}

		return false;
	}

    
	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}

    IEnumerator RefreshFloorTest()
	{
		yield return new WaitForSeconds(0.1f);
		floorTest = false;
	}
}
