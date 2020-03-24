using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    Rigidbody2D rigid;
    bool beingPushed = false;
    [SerializeField] float playerPushForceX = -300f;
    [SerializeField] float playerPushForceY = 50f;

    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!rigid.isKinematic)
        {
            if (other.gameObject.tag == "Deadly")
            {
                rigid.velocity = new Vector2(-100f,100f);
                StartCoroutine(PushCharacter());
            }

            if (other.gameObject.tag == "Player" && beingPushed)
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(playerPushForceX, playerPushForceY);
            }
        }
    }

    IEnumerator PushCharacter()
    {
        beingPushed = true;
        yield return new WaitForSeconds(0.3f);
        beingPushed = false;
    }

    public void Pushed()
    {
        rigid.isKinematic = false;
        rigid.velocity = new Vector2(200f,-200f);
    }
}
