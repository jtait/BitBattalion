using UnityEngine;
using System.Collections;

public class BitShip_1 : BitShip {

    private Transform targetTransform;
    private Vector3 targetPosition;

    protected override void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Boss").transform;

        /* set all basic parameters */
        health = 1;
        pointValue = 200 * difficulty;
        fireRate = fireRate * difficulty;
        baseForwardVelocity = 2;
    }

    /* override base class Move() function */
    protected override void Move()
    {
        float step = baseForwardVelocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    protected override void Death()
    {
        base.Death();
    }
}
