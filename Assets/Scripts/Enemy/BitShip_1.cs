using UnityEngine;
using System.Collections;

public class BitShip_1 : GenericEnemy {

    private Transform targetTransform;

    protected override void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        
        /* set all basic parameters */
        health = 1;
        pointValue = 200 * difficulty;
        fireRate = fireRate * difficulty;
        baseForwardVelocity = 4 * gParams.speedMultiplier;
    }

    void FixedUpdate()
    {
        Move();
    }

    /* override base class Move() function */
    void Move()
    {
        float step = baseForwardVelocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, step);
    }

    protected override void Death()
    {
        base.Death();
    }
}
