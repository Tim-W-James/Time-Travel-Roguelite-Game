using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotationOfVelocity : MonoBehaviour {

    private Rigidbody2D rb2d;               //store a reference to the Rigidbody2D component required to use 2D Physics.

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update () {
        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;    //set angle projectile is facing based on velocity
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
