using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformThreeNodes : MonoBehaviour {

    // You can add up to three nodes to create more complex movements
	[SerializeField] GameObject Node1;
	[SerializeField] GameObject Node2;
    [SerializeField] GameObject Node3;
    // Determines when the platform will start moving
    enum startMoving
	{
		ON_START,
		ON_TRIGGER
	}
	[SerializeField] startMoving startMovingType;
    [SerializeField] float timeBeforeStopMoving = 1;
    // Check if three nodes are specified
    [SerializeField] bool threeNodesActivate;
	[SerializeField] float moveSpeed;
	float step;
	Vector2 target;
	bool triggered = false;
    bool fromNode3;
	
	void FixedUpdate () 
	{
        // Setup move Speed
		step = moveSpeed * Time.fixedDeltaTime;

        // Start moving depending on startMovingType
		switch(startMovingType)
		{
		case startMoving.ON_START:
			ChooseTargetAndMove ();
			break;
		case startMoving.ON_TRIGGER:
			if (triggered) 
			{
				ChooseTargetAndMove ();
			}
			break;
		}
	}

	void ChooseTargetAndMove()
	{
        // Moving between the three nodes if activated
        if (threeNodesActivate)
        {
            if ((Vector2)transform.position == (Vector2)Node1.transform.position)
            {
                target = (Vector2)Node2.transform.position;
                fromNode3 = false;
            }
                
            if ((Vector2)transform.position == (Vector2)Node2.transform.position)
            {
                if (fromNode3)
                {
                    target = (Vector2)Node1.transform.position;
                }
                else
                {
                    target = (Vector2)Node3.transform.position;
                } 
            }
            if ((Vector2)transform.position == (Vector2)Node3.transform.position)
            {
                target = (Vector2)Node2.transform.position;
                fromNode3 = true;
            }
            transform.position = Vector2.MoveTowards(this.transform.position, target, step);
        }

        // Moving between two nodes
        else
        {
            if ((Vector2)this.transform.position == (Vector2)Node1.transform.position)
                target = (Vector2)Node2.transform.position;
            else if ((Vector2)this.transform.position == (Vector2)Node2.transform.position)
                target = (Vector2)Node1.transform.position;
            this.transform.position = Vector2.MoveTowards(this.transform.position, target, step);
        }
		
	}

    // Make sure the player moves accordingly with the platform if they are on it
	void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
        {
            if(!triggered)
                triggered = true;
            if(other.transform.parent != this.transform)
                other.transform.parent = this.transform;
        }	
	}

    // Stop following the platform's movement as soon as the player isn't on it anymore
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
        {
            Invoke("StopMoving", timeBeforeStopMoving);
            other.transform.parent = null;
        }
	}
    
    void StopMoving()
    {
        triggered = false;
    }
}
