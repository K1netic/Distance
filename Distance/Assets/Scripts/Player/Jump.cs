using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour {

	public static bool isGrounded;
	public Transform feetPos;
	public float checkRadius;
	public LayerMask groundLayer;
    Rigidbody2D rigid;

	[SerializeField] float jumpForce;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update () {
        // Test si le personnage est au sol
        isGrounded = Physics2D.OverlapCircle (feetPos.position, checkRadius, groundLayer);

        // Saut
        if (isGrounded && Input.GetButton("Jump"))
		{
			// rigid.velocity += (new Vector2(0, (Vector2.up.y * jumpForce) -Physics2D.gravity.y)) * Time.deltaTime;
            rigid.AddForce(Vector2.up * jumpForce);
		}
	}

}
	