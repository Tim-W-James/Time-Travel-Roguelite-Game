using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

    public GameObject projectile;
    public float spawnRate = 0.5f;
    public float spawnAngle = 0f;
    public float projectileSpeed = 10;
    public int spawnInnacuracy = 3;
    public bool isHoming = false;
    public bool isTargetPlayer = false;

    private Transform homingTarget;
    private GameObject tempProjectile;       //store a reference to the Rigidbody2D component required to use 2D Physics
    private Rigidbody2D tempRigidbody2D;
    private float projectileCooldown;
    private bool isOnCooldown = false;

    private void Start()
    {
        homingTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update () {

        if (!isOnCooldown && Vector2.Distance(transform.position, homingTarget.position) < 50)
        {
            if (isTargetPlayer)
            {
                Vector2 tempPos = homingTarget.position;
                tempPos = tempPos - (Vector2) transform.position;
                spawnAngle = Mathf.Atan2(tempPos.y, tempPos.x) * Mathf.Rad2Deg;
            }

            int tempRange = Random.Range(-spawnInnacuracy, spawnInnacuracy + 1);  //set innacuracy based on range

            tempProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, spawnAngle));
            tempRigidbody2D = tempProjectile.GetComponent<Rigidbody2D>();
            if (!isHoming)
                tempRigidbody2D.AddForce(Quaternion.AngleAxis(spawnAngle + tempRange, Vector3.forward) * Vector3.right * 100 * projectileSpeed); //add force to projectile

            projectileCooldown = Time.time + spawnRate;
            isOnCooldown = true;
        }

        if (Time.time > projectileCooldown && isOnCooldown)
        {
            isOnCooldown = false;
        }
    }
}
