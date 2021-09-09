using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour {

    public float delay = 0f; //delay before animation expires

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, delay); //destroy animator after finished + delay
    }
}
