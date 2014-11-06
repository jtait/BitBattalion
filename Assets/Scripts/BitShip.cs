using UnityEngine;
using System.Collections;

public abstract class BitShip : MonoBehaviour {

    protected float baseForwardVelocity = 0f; // the base speed of the ship, before modifiers
    protected int difficulty; // the difficulty modifier of the ship - useful for endless mode or repeating levels at higher difficulty
    protected GameObject ammunition; // the type of projectile the ship fires
    protected int fireRate = 1; // how often the BitShip fires, after modifiers
    public int pointValue = 0; // the number of points the player gets when the ship is destroyed

    public int health = 1; // how many shots does it take to kill this ship, with modifiers

    public bool special = false; // a special parameter - the use is defined by the child class

    protected bool triggered_1 = false;
    protected bool triggered_2 = false;
    protected bool triggered_3 = false;
    
    protected virtual void Start () {
        difficulty = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>().difficulty;
	}

    void FixedUpdate()
    {
        Move();
        Shoot();
    }

    void Update()
    {
        Death();
    }

    /* basic Move function - moves ship forward (down) at baseForwardVelocity */
    protected virtual void Move()
    {

    }

    /* basic shoot function - spawns new projectile */
    protected virtual void Shoot()
    {
        /* spawn a new projectile */
    }

    /* check for death condition met */
    protected virtual void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    /* check for collisions with projectiles */
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Projectile")
        {
            Destroy(col.gameObject);
            health--;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Trigger1")
        {
            triggered_1 = true;
        }

        if (col.name == "Trigger2")
        {
            triggered_2 = true;
        }

        if (col.name == "Trigger3")
        {
            triggered_3 = true;
        }
    }
	
}
