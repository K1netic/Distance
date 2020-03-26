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

    [SerializeField] bool fallRight = true;

    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // If the barrier is touched by a projectile, it is pushed back
        if (other.gameObject.tag == "Deadly")
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
    }
}
