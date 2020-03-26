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
	public LayerMask grassLayer;
	public static bool isOnGrass;

	//SOUND
	public string inputsound;
	private FMOD.Studio.EventInstance instance;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Tests if player on ground
        isGrounded = checkIfGrounded(groundLayer);
		// Tests if player on grass
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
            // FMODUnity.RuntimeManager.PlayOneShot(inputsound);
			if (PlaybackState(instance) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
			{
				instance = FMODUnity.RuntimeManager.CreateInstance(inputsound);
				instance.start();
			}
            floorTest = true;
		}
    }

	FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
	{
		FMOD.Studio.PLAYBACK_STATE pS;
		instance.getPlaybackState(out pS);
		return pS;
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

		// Raycasts located slightly on the player's left "feet"
		for (int i = 0; i < leftHits.Length; i++)
		{
			RaycastHit2D leftHit = leftHits [i];
			if (leftHit.collider != null)
			{
				return true;
			}
		}

		// Raycasts located slightly on the player's right "feet"
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
