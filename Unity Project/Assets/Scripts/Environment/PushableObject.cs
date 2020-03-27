using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    Rigidbody2D rigid;
    bool beingPushed = false;
    // Forces at which the player will be thrown if they are on the platform when it gets pushed back
    [SerializeField] float playerPushForceX = -300f;
    [SerializeField] float playerPushForceY = 50f;

    // Uncheck if the pushable barrier should fall to the left
    // Check if the pushable barrier should fall to the right
    [SerializeField] public bool fallRight = true;
    // Check so that the pushable barrier isn't affected by forces once pushed by the player
    [SerializeField] bool metalBarrier = false;

    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // If the barrier is touched by a projectile, it is pushed back
        if (other.gameObject.tag == "Deadly" && !metalBarrier)
        {
            rigid.isKinematic = false;
            if (fallRight)
                rigid.velocity = new Vector2(-100f,100f);
            else 
                rigid.velocity = new Vector2(100f,100f);
            StartCoroutine(PushCharacter());
        }

        // If the character is on the barrier and the platform is being pushed by a projectile, the player is pushed too
        if (other.gameObject.tag == "Player" && beingPushed)
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(playerPushForceX, playerPushForceY);
        }

    }

    // Allow for character to be pushed for 0.3 seconds
    IEnumerator PushCharacter()
    {
        beingPushed = true;
        yield return new WaitForSeconds(0.3f);
        beingPushed = false;
    }

    // Used when the player uses a heavy dash on the barrier
    public void Pushed()
    {
        rigid.isKinematic = false;
        if (fallRight)
            rigid.velocity = new Vector2(200f,-200f);
        else
            rigid.velocity = new Vector2(-200f,-200f);
        if (metalBarrier)
            StartCoroutine(ApplyMetalBarrier());
    }

    IEnumerator ApplyMetalBarrier()
    {
        yield return new WaitForSeconds(0.4f);
        rigid.isKinematic = true;
    }
}
