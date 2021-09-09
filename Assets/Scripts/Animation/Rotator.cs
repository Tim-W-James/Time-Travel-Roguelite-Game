using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rotates an object

public class Rotator : MonoBehaviour {

    //update is called every frame
    void Update()
    {
        //rotate thet transform of the game object this is attached to by 45 degrees, taking into account the time elapsed since last frame.
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
    }
}
