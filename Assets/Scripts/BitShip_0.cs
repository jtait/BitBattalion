using UnityEngine;
using System.Collections;

public class BitShip_0 : BitShip {

	// Use this for initialization
	void Start () {
	
	}
	
    protected void Move()
    {
        rigidbody.velocity = new Vector3(0, forwardVelocity, 0);
    }

}
