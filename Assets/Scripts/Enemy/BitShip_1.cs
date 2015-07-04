using UnityEngine;
using System.Collections;

public class BitShip_1 : GenericEnemy, IGenericEnemy {

    private Transform targetTransform;

    protected override void Start()
    {
        base.Start();

        targetTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        
        /* set all basic parameters */
        health = 1;
        pointValue = 200 * gParams.difficulty;
        fireRate = fireRate * gParams.difficulty;
        baseForwardVelocity = 4 * gParams.speedMultiplier;
    }

    void FixedUpdate()
    {
        Move();
    }

    /* override base class Move() function */
    public void Move()
    {
        float step = baseForwardVelocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, step);
    }


    public bool Active()
    {
        return true;
    }

    public bool Enabled()
    {
        throw new System.NotImplementedException();
    }

    public new void Death()
    {
        base.Death();
    }

    public void Shoot()
    {
        throw new System.NotImplementedException();
    }

}
