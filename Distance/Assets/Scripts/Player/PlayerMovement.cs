using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float maxSpeed = 50;

    public static int playerDirection = 1;

    float smoothTime = 0.3f;
    float xVelocity = 0.0f;

    Rigidbody2D rigid;

	public static bool lockMovement;

    // Use this for initialization 
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        //Prevent the player to override velocity during dash
        if (!lockMovement)
        {
            //Handle the player's direction
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
                playerDirection = 1;
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                playerDirection = -1;
            }

            //If there is an input, character starts to move
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                float acceleration = Mathf.SmoothDamp(0, playerDirection * maxSpeed, ref xVelocity, smoothTime);
                rigid.velocity = new Vector2(acceleration, rigid.velocity.y);
            }
            //rigid.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rigid.velocity.y); 
        }
    }
}