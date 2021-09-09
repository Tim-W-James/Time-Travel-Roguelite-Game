using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//move object from current position to target position for platform spawn

public class SmoothMoveToPointPlatformSpawn : MonoBehaviour {

    public Vector3 target;          //target to move towards
    public Sprite completedSprite;  //sprite for completed platform
    public float speed;             //speed approaching target

    private bool isTargetReached = false;         //track if target reached
    private PolygonCollider2D colliderComponent;    //collider for platform edges

    void TestCompleted() //test for completion
    {   
        if (transform.position == target) //check if current position matches target
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Main";   //revert sorting layer
            isTargetReached = true;                                     
            colliderComponent = GetComponent<PolygonCollider2D>();      //disable collider for platform edges
            colliderComponent.enabled = false;
            GetComponent<SpriteRenderer>().sprite = completedSprite;    //change sprite on completion
        }
    }

    void Update()
    {
        if (!isTargetReached)   // check that target has not been reached
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Foreground"; //make platform appear above other objects

            float step = speed * Time.deltaTime;

            target = new Vector3(target.x, target.y, transform.position.z); //set target   
            TestCompleted();    //test for completion

            transform.position = Vector3.MoveTowards(transform.position, target, step); //move towards target          
        }
    }
}
