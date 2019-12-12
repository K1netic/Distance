using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	[SerializeField] GameObject Node1;
	[SerializeField] GameObject Node2;

    enum startMoving
	{
		ON_START,
		ON_TRIGGER
	}
	[SerializeField] startMoving startMovingType;
    [SerializeField] float timeBeforeStopMoving = 1;
    [SerializeField] bool threeNodesActivate;
	[SerializeField] float moveSpeed;
	float step;
	Vector2 target;
	bool triggered = false;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		step = moveSpeed * Time.fixedDeltaTime;

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
        if ((Vector2)this.transform.position == (Vector2)Node1.transform.position)
            target = (Vector2)Node2.transform.position;
        else if ((Vector2)this.transform.position == (Vector2)Node2.transform.position)
            target = (Vector2)Node1.transform.position;
        this.transform.position = Vector2.MoveTowards(this.transform.position, target, step);
	}

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
