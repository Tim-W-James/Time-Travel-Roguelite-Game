using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//move object from current position to target position

public class SmoothMoveToPoint : MonoBehaviour {

    public Vector3 target;              //target to move towards
    public float speed;                 //speed approaching target
    public bool canMoveXAxis = true;    //track if can move on x axis
    public bool canMoveYAxis = true;    //track if can move on y axis

    private bool isTargetReached = false;   //track if target reached

    void TestCompleted() //test for completion
    {   
        if (transform.position == target)   //check if current position matches target
            isTargetReached = true;
    }

    void Update()
    {
        if (isTargetReached == false) //check that target has not been reached
        {
            float step = speed * Time.deltaTime;

            if (canMoveXAxis && canMoveYAxis) //check that can move on both axes
            {
                target = new Vector3(target.x, target.y, transform.position.z); //set target
                TestCompleted();
            }             

            else if (canMoveXAxis) //check that can move on x axis
            {
                target = new Vector3(target.x, 0, 0); //set target
                TestCompleted();
            }

            else   //check that can move on y axis
            {
                target = new Vector3(transform.position.x, target.y, transform.position.z); //set target
                TestCompleted();    //test for completion
            }

            transform.position = Vector3.MoveTowards(transform.position, target, step); //move towards target       
        }
    }
}
