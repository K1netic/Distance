using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour {

	public static bool isGrounded;
	public Transform feetPos;
	public float checkRadius;
	public LayerMask groundLayer;
    Rigidbody2D rigid;

	public float jumpForce = 2.5f; // gravity if the jump button is down
	public float fallForce = 2f; // gravity if the jump button is up

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update () {
        // Test si le personnage est au sol
        isGrounded = Physics2D.OverlapCircle (feetPos.position, checkRadius, groundLayer);
        Debug.Log(isGrounded);

        // Saut
        if (isGrounded && Input.GetButton("Jump"))
		{
			rigid.velocity += Vector2.up * (-Physics2D.gravity.y) * (jumpForce) * Time.deltaTime;
            Debug.Log(rigid.velocity);
		}
	}

}
	