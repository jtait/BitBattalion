using UnityEngine;
using System.Collections;

public abstract class BitShip : MonoBehaviour {

    public float forwardVelocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();
        print("moved");
	}

    void Move()
    {
        
    }

    void Shoot()
    {

    }

}
