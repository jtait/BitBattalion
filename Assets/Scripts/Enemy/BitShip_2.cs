using UnityEngine;
using System.Collections;

public class BitShip_2 : GenericEnemy
{

    protected override void Start()
    {
        /* set all basic parameters */
        health = 1;
        pointValue = 200 * difficulty;
        fireRate = fireRate * difficulty;
        baseForwardVelocity = 3 * difficulty * gParams.speedMultiplier;
    }

    void FixedUpdate()
    {
        Move();
    }

    /* override base class Move() function */
    void Move()
    {
        rigidbody.velocity = Vector3.down * baseForwardVelocity;
    }

    protected override void Death()
    {
        base.Death();
    }
}
