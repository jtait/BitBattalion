using UnityEngine;
using System.Collections;

public abstract class BitShip : GenericEnemy {

    public bool special = false; // a special parameter - the use is defined by the child class to set a given condition

    void FixedUpdate()
    {
        Move();
    }
	
}
