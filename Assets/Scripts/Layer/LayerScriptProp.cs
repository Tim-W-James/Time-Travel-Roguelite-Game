using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sets sorting layer for props and other objects based on y pos

public class LayerScriptProp : MonoBehaviour {

    public int xOffset; //offset for larger props
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1 + xOffset; //set layer -1 + offset
    }
}
