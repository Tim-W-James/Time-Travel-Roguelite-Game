using UnityEngine.Audio;
using UnityEngine;

//class for audio assets to be used with audio manager

[System.Serializable]
public class Sound {

    public string name; //name

    public AudioClip clip; //file clip

    //volume and pitch with range sliders
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;   //choose if audio repeats on finish

    [HideInInspector]
    public AudioSource source;
}
