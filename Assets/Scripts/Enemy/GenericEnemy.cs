using UnityEngine;
using System.Collections;

public abstract class GenericEnemy : MonoBehaviour {

    public int pointValue = 0; // the number of points the player gets when the ship is destroyed

    public int health = 1; // how many shots does it take to kill this ship, with modifiers

    protected float baseForwardVelocity; // the base speed of the ship, before modifiers
    protected GameParameters gParams; // reference to the game parameters object
    protected GameObject ammunition; // the type of projectile the ship fires
    protected float fireRate = 1; // how often the BitShip fires, after modifiers

    public bool special = false; // a special parameter - the use is defined by the child class to set a given condition

    protected bool triggered_1 = false;
    protected bool triggered_2 = false;
    protected bool triggered_3 = false;
    public bool onScreen = false;

    protected GameObject explosionParticles;

    /* sounds */
    AudioClip damageSound;

    protected virtual void Awake()
    {
        damageSound = Resources.Load<AudioClip>("SoundFX/hit/hit");
        explosionParticles = Resources.Load<GameObject>("Particles/EnemyExplosion");
    }

    protected virtual void Start()
    {
        gParams = GameParameters.instance;
    }

    /* check for death condition met */
    protected virtual void Death()
    {
        gParams.UpdateScore(pointValue);
        GameObject.Instantiate(explosionParticles, transform.position, Quaternion.identity); // play explosion particles
        Destroy(gameObject);
    }

    /* check for collisions with projectiles */
    protected virtual void OnCollisionEnter(Collision col)
    {
        /* detect when an enemy is hit by a player weapon */
        if (col.collider.tag == "Bomb")
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 2);
        }
        else if (col.collider.tag == "Weapon")
        {
            health--;
            Destroy(col.collider.gameObject); // destroy the weapon that hit the enemy
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 2);
        }
        /* if the enemy hits a player, destroy the enemy  - don't increase score*/
        if (col.collider.tag == "PlayerCollider")
        {
            Destroy(gameObject);
        }

        if (health <= 0)
        {
            Death();
        }

    }

    /* set trigger values as they are encountered */
    void OnTriggerEnter(Collider col)
    {
        switch (col.name)
        {
            case "Trigger1":
                triggered_1 = true;
                break;
            case "Trigger2":
                triggered_2 = true;
                break;
            case "Trigger3":
                triggered_3 = true;
                break;
            case "TopOfCameraView":
                onScreen = true;
                break;
            case "BottomOfCameraView":
                onScreen = false;
                break;
        }
    }

}
