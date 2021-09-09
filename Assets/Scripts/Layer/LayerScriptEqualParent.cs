using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerScriptEqualParent : MonoBehaviour {

    public int offset = 0;

    void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = PlayerController.instace.GetComponent<SpriteRenderer>().sortingOrder + offset;
    }
}
