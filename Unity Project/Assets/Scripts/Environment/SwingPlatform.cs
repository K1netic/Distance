using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPlatform : MonoBehaviour
{
    GameObject platform; 
    Rigidbody2D rigid;
    void Start()
    {
        platform = transform.GetChild(0).gameObject;
        rigid = platform.GetComponent<Rigidbody2D>();
        // Add a force to make the platform start swinging
        rigid.AddForce(new Vector2(15.0f, 0f), ForceMode2D.Impulse);
    }

}
