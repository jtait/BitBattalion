using UnityEngine;
using System.Collections;

public abstract class BitShip : MonoBehaviour {

    public float baseForwardVelocity; // the base speed of the ship, before modifiers
    public int difficulty = 1; // the difficulty modifier of the ship - useful for endless mode or repeating levels at higher difficulty
    public GameObject ammunition; // the type of projectile the ship fires
    public int baseFireRate; // how often the BitShip fires, without modifiers
    protected int fireRate; // how often the BitShip fires, after modifiers
    public int pointValue; // the number of points the player gets when the ship is destroyed
    protected int basePointValue;
    
	protected virtual void Start () {
        fireRate = baseFireRate * difficulty;
        pointValue = basePointValue * difficulty;
	}

    void FixedUpdate()
    {
        Move();
        Shoot();
    }

    /* basic Move function - moves ship forward (down) at baseForwardVelocity */
    protected virtual void Move()
    {
        rigidbody.velocity = Vector3.down * baseForwardVelocity;
    }

    /* basic shoot function - spawns new projectile */
    protected virtual void Shoot()
    {
        /* spawn a new projectile */
    }
	
}
