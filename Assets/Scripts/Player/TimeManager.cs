using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;                 //controller rumble

public class TimeManager : MonoBehaviour {

    private float currentSlowdownLength = 2f; //track slowdown duration

    private void Start()
    {
        SlowMotion(0.05f, 0.5f); //calibrate time
    }

    private void Update()
    {
        Time.timeScale += (1f / currentSlowdownLength) * Time.unscaledDeltaTime; //revert time back to normal gradually
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f); //clamp time to 1
    }

    public void SlowMotion(float slowdownFactor, float slowdownLength) //slowdown for slowdownFactor amount
    {
        Time.timeScale = slowdownFactor; //set time scale
        Time.fixedDeltaTime = Time.timeScale * .02f; //set delta time for physics
        currentSlowdownLength = slowdownLength;
    }
}
