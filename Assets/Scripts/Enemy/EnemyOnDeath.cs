using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnDeath : MonoBehaviour
{
    public GameObject particle;

    private void Update()
    {
        if (this.GetComponent<EnemyHP>().hp < 1)
        {
            Instantiate(particle, transform.position, Quaternion.Euler(0, 0, 0));  //create particle
            Destroy(this.gameObject);
        }
    }
}
