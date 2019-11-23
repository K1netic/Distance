using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPlatform : MonoBehaviour
{
    GameObject platform; 
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        platform = transform.GetChild(0).gameObject;
        rigid = platform.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(15.0f, 0f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
