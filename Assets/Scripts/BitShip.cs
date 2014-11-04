using UnityEngine;
using System.Collections;

public abstract class BitShip : MonoBehaviour {

    public float baseForwardVelocity;
    public int difficulty = 1;
    public GameObject ammunition;
    public int baseFireRate;
    protected int fireRate;
    
	void Start () {
        fireRate = baseFireRate * difficulty;
	}

    void FixedUpdate()
    {
        Move();
        Shoot();
    }

    /* basic Move function - moves ship forward (down) at baseForwardVelocity */
    protected virtual void Move(){
        rigidbody.velocity = new Vector3(0, -baseForwardVelocity, 0);
    }

    /* basic shoot function - spawns new projectile */
    protected virtual void Shoot()
    {
        /* spawn a new projectile */
    }
	
}
