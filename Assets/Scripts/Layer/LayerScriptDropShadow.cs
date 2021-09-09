using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//positions play drop shadow on layer

public class LayerScriptDropShadow : MonoBehaviour {

    public GameObject player;

	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
    }
}
