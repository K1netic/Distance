using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularPlatform : MonoBehaviour {

    // Position and movment management
	[SerializeField] float moveSpeed;
    // Determines whether or not the platform will move clockwise
    [SerializeField] bool clockWise = true;
    // Randomizes the movement direction
    [SerializeField] bool randomDirection;
    Rigidbody2D rigid;
    Vector3 pivotPos;
    Vector3 vectorToTarget;
    // Choose between 0° and 360° to make the platform start the movement at a particular position on the trajectory 
    [SerializeField] float startPositionInDegree; 
    float radius;
    float angle;
    float t;
    public bool startMoving = true;
    // Check if there are thorns attached to the platform so that they move as well
    [SerializeField] bool thornsAttached = false;

    void Awake () 
    {
        // Setup circular movement with chosen parameters
        t = (12.565f * startPositionInDegree) / 180;
        if (thornsAttached)
            pivotPos = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, 5);
        else pivotPos = transform.parent.transform.position;
        rigid = GetComponent<Rigidbody2D>();
        radius = Vector2.Distance(pivotPos, transform.position);

        if (!clockWise)
            moveSpeed = -moveSpeed;

        if (randomDirection)
        {
            if (Random.Range(0, 2) == 1)
                moveSpeed = -moveSpeed;
        }

        // Movement start
        transform.position = pivotPos + new Vector3(Mathf.Sin(t * moveSpeed) * radius, Mathf.Cos(t * moveSpeed) * radius, 0);
        vectorToTarget = transform.position - pivotPos;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        rigid.MoveRotation(angle);
    }

    void FixedUpdate () 
    {
        // Continuous movement as soon as the platform must move
        if (startMoving)
        {
            t += Time.deltaTime;
            rigid.MovePosition(pivotPos + new Vector3(Mathf.Sin(t * moveSpeed) * radius, Mathf.Cos(t * moveSpeed) * radius, 0));
            vectorToTarget = transform.position - pivotPos;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            rigid.MoveRotation(angle);
        }
    }

    // Make sure the player moves accordingly with the platform if they are on it
    void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
        {
            if(other.transform.parent != this.transform)
                other.transform.parent = this.transform;
        }	
	}

    // Stop following the platform's movement as soon as the player isn't on it anymore
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
	}
}


