using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;                 //controller rumble
using EZCameraShake;                    //camera shake

//move object from current position to target position for player spawn

public class SmoothMoveToPointPlayerSpawn : MonoBehaviour
{

    public Vector3 target;                                  //target to move towards
    public float speed;                                     //speed approaching target
    public GameObject dropShadow;                           //player drop shadow
    public GameObject platform;                             //related platform
    public static SmoothMoveToPointPlayerSpawn spawnInstace;//track instance

    [HideInInspector]
    public bool isTargetReached = false;                                //track if target reached
    private bool hasStartedMoving = false;                              //track if platform has started moving
    private List<Collision2D> collisionList = new List<Collision2D>();  //array for all collisions cancelled while spawning

    private void Start()
    {
        spawnInstace = this;
    }

    void TestCompleted()    //test for completion
    {
        if (platform.transform.position.y == -24.8f && !isTargetReached) //check if current position matches target of platform
        {
            GamePad.SetVibration(0, 0, 0);  //stop rumble
            GetComponent<SpriteRenderer>().sortingLayerName = "Main";   //revert sorting layer
            isTargetReached = true;
            dropShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Main";    //revert sorting layer of player
            foreach (var item in collisionList)                                     //revert collisions of all collisions cancelled while spawning
                Physics2D.IgnoreCollision(item.collider, GetComponent<Collider2D>(), false);
        }
    }

    void Update()
    {
        if (platform.transform.position.y > -7 && !hasStartedMoving) //check if platform has started moving
        {
            //run rumble and shake after level has loaded to prevent timing inconsistencies
            GamePad.SetVibration(0, 0.1f, 0.1f); //set rumble while spawning
            CameraShaker.Instance.ShakeOnce(2.5f, 4f, .1f, 7f);  //set camera shake while spawning
            hasStartedMoving = true;
        }

        if (!isTargetReached)   // check that target has not been reached
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";             //make player appear above other objects
            dropShadow.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";  //make player shadow appear above other objects

            float step = speed * Time.deltaTime;

            target = new Vector3(transform.position.x, target.y, transform.position.z); //set target
            TestCompleted();    //test for completion

            transform.position = Vector3.MoveTowards(transform.position, target, step); //move towards target
        }
    }

    //collisions for environment must be disabled while 'player' is above them
    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Environment")) //check if collided object is environmental
        {
            if (!isTargetReached)   // check that target has not been reached
            {
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());  //ignore collision
                collisionList.Add(other);                                               //add collision to array to be reverted
            }
        }
    }
}
