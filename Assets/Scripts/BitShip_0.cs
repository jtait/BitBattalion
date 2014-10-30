using UnityEngine;
using System.Collections;

public class BitShip_0 : BitShip {

    void FixedUpdate()
    {
        Move();
    }

    protected void Move()
    {
        rigidbody.velocity = new Vector3(0, -forwardVelocity, 0);
    }

}
