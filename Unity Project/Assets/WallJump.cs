using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    float detectDistance = 0.5f;
    float detectRange = 1.5f;
    public LayerMask wallLayer;

    bool isOnLeftWall = false;
    bool isOnRightWall = false;
    Jump jumpScript;
    float yVelocity = 0f;
    Rigidbody2D rigid;
    float wallJumpHorizontalForce = 40f;
    float wallJumpVerticalForce = 50f;
    float jumpDuration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        jumpScript = gameObject.GetComponent<Jump>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isOnLeftWall = checkIfOnLeftWall();
        isOnRightWall = checkIfOnRightWall();

        //Si collé à un mur à gauche
        if ((Input.GetAxisRaw("Horizontal") < 0) && isOnLeftWall)
        {
            //Anim perso qui regarde vers la droite
            //Faire glisser perso + son frottements
        }

        if(Input.GetButtonDown("Jump") && isOnLeftWall)
        {
            Debug.Log("diagonal jump");
            PlayerMovement.lockMovement = true;
            // float acceleration = Mathf.SmoothDamp(0, 1 * wallJumpHorizontalForce, ref yVelocity, jumpDuration, wallJumpHorizontalForce);
            rigid.velocity = new Vector2(wallJumpHorizontalForce, wallJumpVerticalForce);
            transform.localScale = new Vector3(1, 1, 0);
            StartCoroutine(UnLockMovement());
        }

        //Si collé à un mur à droite
        if ((Input.GetAxisRaw("Horizontal") > 0) && isOnRightWall)
        {
            //Anim perso qui regarde vers la gauche
            //Faire glisser perso + son frottements
        }

        // Saut en diagonal
        if(Input.GetButtonDown("Jump") && isOnRightWall)
        {
            float acceleration = Mathf.SmoothDamp(0, 1 * wallJumpVerticalForce, ref yVelocity, jumpDuration, wallJumpVerticalForce);
            rigid.velocity = new Vector2(-wallJumpHorizontalForce, wallJumpVerticalForce);
        }
    }

    bool checkIfOnLeftWall()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		Vector2 leftDirection = Vector2.left;
		Debug.DrawRay (new Vector2(position.x - detectRange, position.y), leftDirection * detectDistance, Color.red, 5f);
		RaycastHit2D[] leftHits = Physics2D.RaycastAll(new Vector2(position.x - detectRange, position.y), leftDirection, detectDistance, wallLayer);

		for (int i = 0; i < leftHits.Length; i++)
		{
			RaycastHit2D leftHit = leftHits [i];
			if (leftHit.collider != null)
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
		//Raycasts
		RaycastHit2D[] rightHits = Physics2D.RaycastAll(new Vector2(position.x + detectRange, position.y), rightDirection, detectDistance, wallLayer);


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

    IEnumerator UnLockMovement()
    {
        yield return new WaitUntil(() => rigid.velocity.x == 0);
        PlayerMovement.lockMovement = false;
    }

}
