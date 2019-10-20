using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static int playerDirection = 1;

    [SerializeField] float accelerationSmoothTime = 0.3f;
    [SerializeField] float topSpeed = 20f;
    [SerializeField] float decelerationSmoothTime = 0.1f;
    float xVelocity = 0.0f;

    Rigidbody2D rigid;

    // Utilisé pour bloquer le mouvement du personnage
	public static bool lockMovement;

    // Use this for initialization 
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        if (!lockMovement)
        {
            //Gérer la direction de mouvement
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

            //Commencer un déplacement si un input horizontal est enregistré (ACCELERATION)
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                float acceleration = Mathf.SmoothDamp(0, playerDirection * topSpeed, ref xVelocity, accelerationSmoothTime, topSpeed);
                rigid.velocity = new Vector2(acceleration, rigid.velocity.y);
                Debug.Log(rigid.velocity.y);
            }

            //Arrêter le mouvement du personnage quand l'input est relâché (DECELERATION)
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                float deceleration = Mathf.SmoothDamp(rigid.velocity.x, 0, ref xVelocity, decelerationSmoothTime, topSpeed);
                rigid.velocity = new Vector2(deceleration, rigid.velocity.y);
            }
        }
    }
}