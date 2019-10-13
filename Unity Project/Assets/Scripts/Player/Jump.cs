using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour {

	public static bool isGrounded;
    float groundDetectDistance = 0.01f;
	float groundDetectRange = 1f;
	public Transform feetPos;
	public LayerMask groundLayer;
    Rigidbody2D rigid;

	[SerializeField] float jumpForce;
	float yVelocity = 0f;
	[SerializeField] float jumpVelocityThreshold;

	// Saut dans l'air
	bool airJumpUnlocked = true;
	float airJumpForce;
	bool airJumpAvailable = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
		airJumpForce = jumpForce * 0.66f;
    }

    void Update () {
        // Test si le personnage est au sol
        isGrounded = checkIfGrounded();
		if (isGrounded) airJumpUnlocked = true;

        // Saut
        if (isGrounded && Input.GetButton("Jump") && rigid.velocity.y < jumpVelocityThreshold)
		{
			float acceleration = Mathf.SmoothDamp(0, 1 * jumpForce, ref yVelocity, 0.3f, jumpForce);
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
		}

		// Saut II : le retour
		if (airJumpUnlocked && !isGrounded && Input.GetButton("Jump") && !airJumpUnlocked)
		{
			float acceleration = Mathf.SmoothDamp(0, 1 * jumpForce, ref yVelocity, 0.3f, jumpForce);
			airJumpUnlocked = false;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
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

}
	