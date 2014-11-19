using UnityEngine;
using System.Collections;

public class BitShip_0 : BitShip {

    public float strafeForce = 100f;

    protected override void Start()
    {
        /* set all basic parameters */
        health = 1;
        pointValue = 200 * difficulty;
        fireRate = fireRate * difficulty;
        baseForwardVelocity = 2;

        if (special) strafeForce *= -1f;
    }

    /* override base class Move() function */
    protected override void Move()
    {
        rigidbody.velocity = Vector3.down * baseForwardVelocity; // move forward at constant velocity

        if (triggered_1)
        {
            rigidbody.AddForce(Vector3.right * strafeForce); // strafe when triggered
        }

    }

    protected override void Death()
    {
        base.Death();
    }

}
