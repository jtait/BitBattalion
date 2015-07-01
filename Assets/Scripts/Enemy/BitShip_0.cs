﻿using UnityEngine;
using System.Collections;

public class BitShip_0 : GenericEnemy {

    public float strafeForce = 100f;

    protected override void Start()
    {
        base.Start();

        /* set all basic parameters */
        health = 1;
        pointValue = 200 * difficulty;
        baseForwardVelocity = 2 * gParams.speedMultiplier;

        if (special) strafeForce *= -1f;
    }

    void FixedUpdate()
    {
        this.Move();
    }

    /* override base class Move() function */
    void Move()
    {
        GetComponent<Rigidbody>().velocity = Vector3.down * baseForwardVelocity; // move forward at constant velocity

        if (triggered_1)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.right * strafeForce); // strafe when triggered
        }

    }

    /* check for collisions with projectiles */
    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);

        if (col.collider.tag == "Wall")
        {
            health = 0;
        }

    }

}
