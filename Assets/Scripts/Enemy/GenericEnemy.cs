using UnityEngine;
using System.Collections;

public abstract class GenericEnemy : MonoBehaviour {

    public int pointValue = 0; // the number of points the player gets when the ship is destroyed

    public int health = 1; // how many shots does it take to kill this ship, with modifiers

    protected float baseForwardVelocity = 0f; // the base speed of the ship, before modifiers
    protected GameParameters gParams;
    protected int difficulty; // the difficulty modifier of the enemy - useful for endless mode or repeating levels at higher difficulty
    public GameObject ammunition; // the type of projectile the ship fires
    protected float fireRate = 1; // how often the BitShip fires, after modifiers

    public bool special = false; // a special parameter - the use is defined by the child class to set a given condition

    protected bool triggered_1 = false;
    protected bool triggered_2 = false;
    protected bool triggered_3 = false;

    void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        difficulty = gParams.difficulty; // set the difficulty parameter of the enemy to the difficulty of the game when the enemy is created
    }

    protected virtual void Start()
    {
        /* initialization is done in child classes */
    }

    void Update()
    {
        Death(); // check for death condition every frame
    }

    /* basic Move function - moves ship forward (down) at baseForwardVelocity */
    protected virtual void Move()
    {
        /* movement of enemies is done in child classes */
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
            gParams.UpdateScore(pointValue);
            Destroy(gameObject);
        }
    }

    /* check for collisions with projectiles */
    void OnCollisionEnter(Collision col)
    {
        /* detect when an enemy is hit by a player weapon */
        if (col.collider.tag == "Weapon")
        {
            health--;
            Destroy(col.collider.gameObject); // destroy the weapon that hit the enemy
        }
    }

    /* set trigger values as they are encountered */
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
