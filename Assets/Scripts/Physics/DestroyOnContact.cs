using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public GameObject explosion;
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))   //check collision with enemy
        {
            other.gameObject.GetComponent<EnemyHP>().hp -= damage;
            Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));  //create explosion
        }

    }
}
