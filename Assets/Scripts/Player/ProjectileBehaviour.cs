using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages the behaviour for the player projectile

public class ProjectileBehaviour : MonoBehaviour {

    public float projectileDuration = 0.5f; //duration before projectile despawns
    public GameObject explosion;            //particle for explosion

    private Rigidbody2D rb2d;               //store a reference to the Rigidbody2D component required to use 2D Physics.
    private SpriteRenderer spriteRC;
    private Collider2D collder2DC;
    private float enabledTime;
    private float enabledDelay = 0.01f;   //delay before projectile appears
    private bool isActive = false;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, projectileDuration);   //destroy after duration
        spriteRC = gameObject.GetComponent<SpriteRenderer>();
        collder2DC = gameObject.GetComponent<Collider2D>();
        enabledTime = Time.time + enabledDelay;

        spriteRC.enabled = false;   //initially disable sprite renderer component
        collder2DC.enabled = false; //initially disable collider 2D component
    }

    void Update () {        
        if (Time.time > enabledTime && !isActive)   //check if starting duration has expried
        {
            spriteRC.enabled = true;    //enable sprite renderer component
            collder2DC.enabled = true;  //enable collider 2D component
            isActive = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))   //check collision with enemy
        {
            other.gameObject.GetComponent<EnemyHP>().hp--;
            Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));  //create explosion
            rb2d.velocity = new Vector3(0, 0, 0);
            spriteRC.enabled = false;
            Destroy(this.gameObject);   //destroy self
        }

        if (other.gameObject.CompareTag("Environment")) //check collision with environment
        {
            Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));  //create explosion
            rb2d.velocity = new Vector3(0, 0, 0);
            spriteRC.enabled = false;
            Destroy(this.gameObject);   //destroy self
        }
    }
}
