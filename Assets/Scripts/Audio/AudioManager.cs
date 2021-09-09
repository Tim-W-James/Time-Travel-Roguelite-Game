using UnityEngine.Audio;
using System;
using UnityEngine;

//Manages and organises audio

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;  //creates array of sound class in inspector

    public static AudioManager instance; //track instance

	// Use this for initialization
	void Awake () {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); //remaind consistent across scenes to prevent audio being interrupted

		foreach (Sound s in sounds)
        {
            //set parameters for sound class in array
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}

    private void Start()
    {
        //ambient music on start
        Play("Ambient1");
        Play("Ambient2");
    }

    private void FixedUpdate()
    {
        if (PlayerController.instace.isTimeSlowed)
        {
            foreach (Sound s in sounds)
            {
                s.source.pitch = Time.timeScale;
            }
        }      
        
        else
        {
            foreach (Sound s in sounds)
            {
                s.source.pitch = 1;
            }
        }
    }

    public void Play (string name)  //plays sound of name
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); //search array for sound
        if (s == null) //checks if sound exists
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play(); //play sound
    }

    public void Stop(string name) //stop sound of name
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); //search array for sound
        if (s == null) //checks if sound exists
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop(); //stop sound
    }
}
