using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//positions particle on correct layer

public class LayerScriptParticle : MonoBehaviour {

    public int offset = 0; //offset for larger particles

    private ParticleSystemRenderer render; //particle renderer

	// Use this for initialization
	void Start () {
        render = GetComponent<ParticleSystemRenderer>(); //set renderer
    }
	
	// Update is called once per frame
	void Update () {
        render.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1 + offset; //set sorting layer for particle renderer
    }
}
