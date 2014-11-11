using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    private const float BASE_FIRE_RATE = 0.5f;
    private const float FIRE_RATE_PERK = 5f;
    private const float MISSILE_FIRE_RATE = 1f;

    // movement
    public float strafeForce = 10f;
    private float strafeDirection;
    private float velocityDirection;
    public float velocityAddition = 2f;

    // weapons
    private GameObject ammo; // the ammo the player ship will fire
    private GameObject specialWeapon; // the special weapon (bomb, missile, etc.) that the player ship has
    private float fireRate; // delay between regular shots
    private float nextShot; // delay until next regualer shot can be fired
    private float specialFireRate; // delay between special shots
    private float nextSpecial; // delay until next special shot can be fired

    // persistent game parameters
    private GameParameters gameParams;

    // types of available ammo
    private enum WeaponType { laser, missile, bomb, none };
    private GameObject laser;
    private GameObject missile;
    private GameObject bomb;

    private bool shielded = false;
    private WeaponType specialWeaponType;

    void Awake()
    {
        laser = Resources.Load<GameObject>("Ammo/LaserShot");
        missile = Resources.Load<GameObject>("Ammo/Missile");
        bomb = Resources.Load<GameObject>("Ammo/Bomb");
        ammo = laser;
        nextShot = 0f;
        nextSpecial = 0f;
        gameParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        fireRate = BASE_FIRE_RATE;
    }
    

    void FixedUpdate()
    {

        // strafing
        strafeDirection = Input.GetAxis("Horizontal");
        rigidbody.AddForce(new Vector3(strafeDirection * strafeForce, 0, 0));

        // forward / backward movement
        velocityDirection = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector3(0, velocityAddition * velocityDirection, 0);

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Enemy")
        {
            PlayerDeath();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShootSpecialWeapon();
        }
    }

    // create and launch projectile from ships location
    void Shoot()
    {
        if (Time.time > nextShot)
        {
            // generate a new object to fire, instantiate with velocity, power, etc.
            Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject clone = GameObject.Instantiate(ammo, launchFrom, Quaternion.identity) as GameObject;
            clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
            nextShot = Time.time + fireRate;
        }
    }

    // create and fire a special weapon if the player has one
    void ShootSpecialWeapon()
    {
        if (specialWeapon != null)
        {
            if (Time.time > nextSpecial)
            {
                switch (specialWeaponType)
                {
                    case WeaponType.bomb:
                        //TODO
                        // launch bomb here
                        break;
                    case WeaponType.missile:
                        // generate a new missile to fire, instantiate with velocity, power, etc.
                        Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                        GameObject clone = GameObject.Instantiate(ammo, launchFrom, Quaternion.identity) as GameObject;
                        clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
                        nextSpecial = Time.time + fireRate;
                        break;
                }
            }
        }
    }

    void PlayerDeath()
    {
        /* decrease player lives */
        gameParams.playerLives--;

        /* reset powerUp benefits */
        PowerUpPickup(PowerUpType.None);

        if (gameParams.playerLives > 0)
        {
            /* destroy all enemies */
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in toDestroy)
            {
                Destroy(enemy);
            }

            /* move player to last checkpoint */
            transform.parent.position = gameParams.lastCheckpoint;

        }
        else
        {
            //TODO
            /* GAME OVER */
        }

    }

    void OnTriggerEnter(Collider col)
    {
        /* update checkpoints as the player reaches them */
        if (col.tag == "CheckPoint")
        {
            gameParams.lastCheckpoint = col.transform.position;
        }
    }

    public void PowerUpPickup(PowerUpType type)
    {
        switch(type){
            case PowerUpType.None:
                ammo = laser;
                specialWeaponType = WeaponType.none;
                shielded = false;
                fireRate = BASE_FIRE_RATE;
                break;
            case PowerUpType.Missile:
                specialWeaponType = WeaponType.missile;
                specialFireRate = MISSILE_FIRE_RATE;
                break;
            case PowerUpType.Bomb:
                specialWeaponType = WeaponType.bomb;
                break;
            case PowerUpType.ExtraLife:
                gameParams.playerLives++;
                break;
            case PowerUpType.RapidFire:
                fireRate *= FIRE_RATE_PERK;
                break;
            case PowerUpType.Shield:
                shielded = true;
                break;
        }
        
    }

}
