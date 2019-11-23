using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    Rigidbody2D rigid;
    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;
    }

    public void Pushed()
    {
        rigid.isKinematic = false;
        rigid.velocity = new Vector2(200f,-200f);
    }
}
