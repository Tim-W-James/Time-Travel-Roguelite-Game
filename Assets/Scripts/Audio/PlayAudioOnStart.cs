using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour {

    public string sound;

	// Use this for initialization
	void Start () {
        FindObjectOfType<AudioManager>().Play(sound);
    }
}
