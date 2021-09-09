using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehavior : MonoBehaviour {

    public float projectileDuration = 0.5f; //duration before projectile despawns
    public GameObject explosion;            //particle for explosion
    public bool isHoming = false;
    public float homingSpeed = 3f;

    private Transform homingTarget;
    private Rigidbody2D rb2d;               //store a reference to the Rigidbody2D component required to use 2D Physics.
    private SpriteRenderer spriteRC;
    private Vector3 mousePos;           //tracks mouse pos
    private bool isReflected;
    private float destroyTime;

    void Start()
    {
        destroyTime = Time.time + projectileDuration;
        rb2d = GetComponent<Rigidbody2D>();
        spriteRC = gameObject.GetComponent<SpriteRenderer>();
        homingTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    float CalcReflectAngle(bool gamePadMode)   //calculates angle player is aiming
    {
        float tempAngle = 0;

        if (gamePadMode)    //game pad uses angle on joystick
        {
            if (Mathf.Abs(Input.GetAxis("xAim")) == 0 && Mathf.Abs(Input.GetAxis("yAim")) == 0)   //check for game pad mode and no joystick input
            {
                //default angle for last direction facing
                if (PlayerController.instace.directionFacing == 1)
                    tempAngle = 180;

                else if (PlayerController.instace.directionFacing == 2)
                    tempAngle = 90;

                else if (PlayerController.instace.directionFacing == 3)
                    tempAngle = -90;

                else
                    tempAngle = 0;
            }

            else
                tempAngle = Mathf.Atan2(Input.GetAxis("yAim"), Input.GetAxis("xAim")) * Mathf.Rad2Deg;
        }

        else //calculates angle between reflect pos and mouse
        {
            mousePos = Input.mousePosition;
            mousePos.z = (transform.position.z - Camera.main.transform.position.z);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos = mousePos - transform.position;
            tempAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        }

        return tempAngle;
    }

    private void Update()
    {
        if (Time.time > destroyTime)
        {
            Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));  //create explosion
            rb2d.velocity = new Vector3(0, 0, 0);
            spriteRC.enabled = false;
            Destroy(this.gameObject);   //destroy self
        }

        if (isHoming && !isReflected)
        {
            //rotate to look at the player
            transform.LookAt(homingTarget.position);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self); //correcting the original rotation
            
            //move towards the player
            if (Vector3.Distance(transform.position, homingTarget.position) > 1f)
            {//move if distance from target is greater than 1
                transform.Translate(new Vector3(homingSpeed * Time.deltaTime, 0, 0));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("PlayerProjectile"))
        {
            if (other.gameObject.CompareTag("Enemy"))   //check collision with enemy
            {
                Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));  //create explosion
                other.gameObject.GetComponent<EnemyHP>().hp--;
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
        
        else
        {
            if (other.gameObject.CompareTag("PlayerReflect"))   //check collision with player
            {
                destroyTime = Time.time + projectileDuration;
                isReflected = true;
                rb2d.velocity = new Vector3(0, 0, 0);
                rb2d.AddForce((Quaternion.AngleAxis(CalcReflectAngle(PlayerController.instace.isGamePadModeAim), Vector3.forward) * Vector3.right) * 1600 * 25);
                tag = "PlayerProjectile";
            }

            if (other.gameObject.CompareTag("PlayerBurst"))   //check collision with player
            {
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

            if (other.gameObject.CompareTag("Player"))   //check collision with player
            {
                if (PlayerController.instace.canPlayerTakeDamage)
                {
                    Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));  //create explosion
                    rb2d.velocity = new Vector3(0, 0, 0);
                    spriteRC.enabled = false;
                }
                else
                {
                    Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));  //create explosion
                    rb2d.velocity = new Vector3(0, 0, 0);
                    spriteRC.enabled = false;
                    Destroy(this.gameObject);   //destroy self
                }
            }
        }
    }
}