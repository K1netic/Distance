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
    [FMODUnity.EventRef]
    public string inputsound;
	public LayerMask grassLayer;
	public static bool isOnGrass;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Test si le personnage est au sol
        isGrounded = checkIfGrounded(groundLayer);
		// Teste si le personnage marche sur de la terre
		isOnGrass = checkIfGrounded(grassLayer);

        // Falling
		if (!isGrounded && !isOnGrass) 
		{
			playerAnimator.SetBool("falling", true);
			StartCoroutine(RefreshFloorTest());
		}

		// Check if falling on floor
		if ((checkIfGrounded(groundLayer) || checkIfGrounded(grassLayer)) && !floorTest)
		{
			playerAnimator.SetBool("jumping", false);
			playerAnimator.SetBool("falling", false);
        	StartCoroutine(CancelVibration (Vibrations.PlayVibration("FallingOnFloor")));
            FMODUnity.RuntimeManager.PlayOneShot(inputsound);
            floorTest = true;
		}
    }
    
    bool checkIfGrounded(LayerMask layerToTest) 
	{
		Vector2 position = new Vector2(feetPos.position.x, feetPos.position.y);
		Vector2 direction = Vector2.down;

		Debug.DrawRay (new Vector2(position.x - groundDetectRange, position.y), direction * groundDetectDistance, Color.cyan);
		Debug.DrawRay (new Vector2(position.x + groundDetectRange, position.y), direction * groundDetectDistance, Color.cyan);
		//Raycasts
		RaycastHit2D[] leftHits = Physics2D.RaycastAll(new Vector2(position.x - groundDetectRange, position.y), direction, groundDetectDistance, layerToTest);
		RaycastHit2D[] rightHits = Physics2D.RaycastAll(new Vector2(position.x + groundDetectRange, position.y), direction, groundDetectDistance, layerToTest);

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
