using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotativePlatform : MonoBehaviour {

    [SerializeField] float speed = 20;
    // Determine whether the platform will rotate clockwise or not
    [SerializeField] bool clockwise = true;
    // Randomizes rotation direction
    [SerializeField] bool randomDirection;

    Rigidbody2D rigid;
    float t;

    private void Start()
    {
        // Setup movement properties
        rigid = GetComponent<Rigidbody2D>();
        if (rigid.bodyType != RigidbodyType2D.Kinematic)
            rigid.bodyType = RigidbodyType2D.Kinematic;
        if (clockwise)
            speed = -speed;
        if (randomDirection)
        {
            if (Random.Range(0, 2) == 1)
                speed = -speed;      
        }
    }

    private void Update()
    {
        // Rotate paltform on itself
        t += Time.deltaTime;
        rigid.MoveRotation(speed*t);
    }
}
