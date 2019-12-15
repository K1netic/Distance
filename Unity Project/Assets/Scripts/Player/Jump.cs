using System.Collections;
using UnityEngine;
using XInputDotNetPure;

public class Jump : MonoBehaviour {

    Rigidbody2D rigid;

	[SerializeField] public float jumpForce = 70f;
	float yVelocity = 0f;
	[SerializeField] float jumpVelocityThreshold = 15f;
    [FMODUnity.EventRef]
    public string inputsound;

    Animator playerAnimator;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
		playerAnimator = gameObject.GetComponent<Animator>();
    }

    void Update () {

        // Saut
        if ((GroundCheck.isGrounded || GroundCheck.isOnGrass) && Input.GetButtonDown("Jump") && rigid.velocity.y < jumpVelocityThreshold)
		{
			playerAnimator.SetBool("jumping", true);
            FMODUnity.RuntimeManager.PlayOneShot(inputsound);
            // float acceleration = Mathf.SmoothDamp(0, 1 * jumpForce, ref yVelocity, 0.3f, jumpForce);
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
			StartCoroutine(RefreshFloorTest());
		}
	}

	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
	}

	IEnumerator RefreshFloorTest()
	{
		yield return new WaitForSeconds(0.1f);
		GroundCheck.floorTest = false;
	}
}
	