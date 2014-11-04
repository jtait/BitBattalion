using UnityEngine;
using System.Collections;

public class BitShip_0 : BitShip {

    private int moveTimer = 100;
    public float moveForce = 100f;

    protected override void Start()
    {
        pointValue = 200;
        fireRate = baseFireRate * difficulty;
    }

    /* override base class Move() function */
    protected override void Move()
    {
        base.Move();

        if (moveTimer > 0)
        {
            moveTimer--;
        }
        else
        {
            rigidbody.AddForce(Vector3.right * moveForce);
        }

    }

    protected override void Shoot()
    {
        //base.Shoot();
    }

}
