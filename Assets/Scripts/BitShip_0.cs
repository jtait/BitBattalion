using UnityEngine;
using System.Collections;

public class BitShip_0 : BitShip {

    /* override base class Move() function */
    protected override void Move()
    {
        base.Move();
    }

    protected override void Shoot()
    {
        //base.Shoot();
    }


    protected override void SetScore()
    {
        pointValue = 200;
    }

}
