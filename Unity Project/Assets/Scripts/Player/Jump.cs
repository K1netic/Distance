using System.Collections;
using UnityEngine;
using XInputDotNetPure;

public class Jump : MonoBehaviour {

	public static bool isGrounded;
    float groundDetectDistance = 0.01f;
	float groundDetectRange = 1f;
	public Transform feetPos;
	public LayerMask groundLayer;
    Rigidbody2D rigid;

	[SerializeField] public float jumpForce = 70f;
	float yVelocity = 0f;
	[SerializeField] float jumpVelocityThreshold = 15f;

	bool floorTest = false;

	Animator playerAnimator;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
		playerAnimator = gameObject.GetComponent<Animator>();
    }

    void Update () {
        // Test si le personnage est au sol
        isGrounded = checkIfGrounded();

        // Saut
        if (isGrounded && Input.GetButtonDown("Jump") && rigid.velocity.y < jumpVelocityThreshold)
		{
			playerAnimator.SetBool("jumping", true);
			// float acceleration = Mathf.SmoothDamp(0, 1 * jumpForce, ref yVelocity, 0.3f, jumpForce);
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
			StartCoroutine(RefreshFloorTest());
		}

		// Check if falling on floor
		if (checkIfGrounded() && !floorTest)
		{
			playerAnimator.SetBool("jumping", false);
			playerAnimator.SetBool("falling", false);
        	StartCoroutine(CancelVibration (Vibrations.PlayVibration("FallingOnFloor")));
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
	