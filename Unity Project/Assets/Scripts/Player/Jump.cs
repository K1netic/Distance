using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour {

	public static bool isGrounded;
    [SerializeField] float groundDetectDistance;
	[SerializeField] float groundDetectRange;
	public Transform feetPos;
	public LayerMask groundLayer;
    Rigidbody2D rigid;

	[SerializeField] float jumpForce;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update () {
        // Test si le personnage est au sol
        isGrounded = checkIfGrounded();
        // isGrounded = Physics2D.OverlapCircle (feetPos.position, checkRadius, groundLayer);
        Debug.Log(isGrounded);
        // Saut
        if (isGrounded && Input.GetButton("Jump"))
		{
            rigid.AddForce(Vector2.up * jumpForce);
		}
	}

    bool checkIfGrounded() 
	{
		Vector2 position = new Vector2(feetPos.position.x, feetPos.position.y);
		Vector2 direction = Vector2.down;

		Debug.DrawRay (new Vector2(position.x - groundDetectRange, position.y), direction, Color.cyan);
		Debug.DrawRay (new Vector2(position.x, position.y), direction, Color.cyan);
		Debug.DrawRay (new Vector2(position.x + groundDetectRange, position.y), direction, Color.cyan);
		//Raycasts
		RaycastHit2D[] leftHits = Physics2D.RaycastAll(new Vector2(position.x - groundDetectRange, position.y), direction, groundDetectDistance, groundLayer);
		// RaycastHit2D[] middleHits = Physics2D.RaycastAll(new Vector2(position.x, position.y), direction, groundDetectDistance, groundLayer);
		RaycastHit2D[] rightHits = Physics2D.RaycastAll(new Vector2(position.x + groundDetectRange, position.y), direction, groundDetectDistance, groundLayer);

		for (int i = 0; i < leftHits.Length; i++)
		{
			RaycastHit2D leftHit = leftHits [i];
			if (leftHit.collider != null)
			{
				return true;
			}
		}

		// for (int i = 0; i < middleHits.Length; i++)
		// {
		// 	RaycastHit2D middleHit = middleHits [i];
		// 	if (middleHit.collider != null)
		// 	{
		// 		return true;
		// 	}
		// }

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
	