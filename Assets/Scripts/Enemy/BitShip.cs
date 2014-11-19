using UnityEngine;
using System.Collections;

public abstract class BitShip : GenericEnemy {

    void FixedUpdate()
    {
        Move();
    }
	
}
