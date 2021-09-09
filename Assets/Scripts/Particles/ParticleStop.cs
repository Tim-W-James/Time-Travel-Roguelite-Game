using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stops particle emission after duration

public class ParticleStop : MonoBehaviour {

    public float timeToStop = 5F; //time particle stops

    private ParticleSystem system;
    private float timeCurrent = 0.0F;

    // Use this for initialization
    void Start () {
        system = GetComponent<ParticleSystem>();
        timeCurrent = Time.time + timeToStop; //calculate time
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > timeCurrent) //check if time has reached time to stop
        {
            system.Stop(); //stop particle emission
        }        
    }
}
