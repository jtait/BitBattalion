using UnityEngine;
using System.Collections;

public class BitShip_2 : GenericEnemy, IGenericEnemy
{

    protected override void Start()
    {
        base.Start();

        /* set all basic parameters */
        health = 1;
        pointValue = 200 * gParams.difficulty;
        fireRate = fireRate * gParams.difficulty;
        baseForwardVelocity = 3 * gParams.difficulty * gParams.speedMultiplier;
    }

    void FixedUpdate()
    {
        Move();
    }

    /* override base class Move() function */
    public void Move()
    {
        GetComponent<Rigidbody>().velocity = Vector3.down * baseForwardVelocity;
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
