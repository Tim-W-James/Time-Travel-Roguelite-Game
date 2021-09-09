using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSlimeOnDeath : MonoBehaviour
{
    public GameObject particle;
    public GameObject redSlime;
    
    private void Update()
    {
        if (this.GetComponent<EnemyHP>().hp < 1) {
            Instantiate(particle, transform.position, Quaternion.Euler(0, 0, 0));  //create particle
            Instantiate(redSlime, transform.position + new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));  //create slime
            Instantiate(redSlime, transform.position + new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));  //create slime
            Instantiate(redSlime, transform.position + new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));  //create slime
            Destroy(this.gameObject);
        }
    }
}
