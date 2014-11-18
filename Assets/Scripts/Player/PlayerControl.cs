using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    private const float BASE_FIRE_RATE = 0.3f; // the base fire rate of the ship.  Lower is faster.
    private const float FIRE_RATE_PERK = 0.1f; // the rapid fire rate of the ship
    private const float MISSILE_FIRE_RATE = 1f; // the missile fire rate
    private const float SHOT_OFFSET = 2.5f;
    private const float RAPID_FIRE_TIMER = 10f; // the length of time for rapid fire to last

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
    private int specialWeaponAmmoCount;
    private bool rapidFireEnabled;
    private float rapidFireTimer;

    /* initialize parameters here */
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
        specialWeaponType = WeaponType.none;
        specialWeaponAmmoCount = 0;
        rapidFireEnabled = false;
        rapidFireTimer = 0f;
    }
    
    void FixedUpdate()
    {
        /* strafing */
        strafeDirection = Input.GetAxis("Horizontal");
        rigidbody.AddForce(new Vector3(strafeDirection * strafeForce, 0, 0));

        /* forward and backward movement */
        velocityDirection = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector3(0, velocityAddition * velocityDirection, 0);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Enemy")
        {
            if (shielded) shielded = false;
            else PlayerDeath();
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
            ShootSpecialWeapon(specialWeapon);
        }

        if (rapidFireEnabled)
        {
            rapidFireTimer -= Time.deltaTime;
            if (rapidFireTimer < 0)
            {
                SetRapidFire(false);
            }
        }


    }

    /* create and launch projectile from ships location */
    void Shoot()
    {
        if (Time.time > nextShot)
        {
            // generate a new object to fire, instantiate with velocity, power, etc.
            Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + SHOT_OFFSET, transform.position.z);
            GameObject clone = GameObject.Instantiate(ammo, launchFrom, Quaternion.identity) as GameObject;
            clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
            nextShot = Time.time + fireRate;
        }
    }

    /* create and fire a special weapon if the player has one */
    void ShootSpecialWeapon(GameObject type)
    {
        if (specialWeapon != null)
        {
            if (Time.time > nextSpecial)
            {
                Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + SHOT_OFFSET, transform.position.z);
                GameObject clone = GameObject.Instantiate(type, launchFrom, Quaternion.identity) as GameObject;
                clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
                nextSpecial = Time.time + fireRate;
                specialWeaponAmmoCount--;
                if (specialWeaponAmmoCount <= 0)
                    SetSpecialWeapon(WeaponType.none);
            }
        }
    }

    /* called when a player dies */
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

    /* set the special weapon parameters to the specified WeaponType */
    private void SetSpecialWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.none:
                specialWeaponType = WeaponType.none;
                specialWeapon = null;
                specialWeaponAmmoCount = 0;
                break;
            case WeaponType.missile:
                specialWeaponType = WeaponType.missile;
                specialWeapon = missile;
                specialWeaponAmmoCount = 3;
                break;
            case WeaponType.bomb:
                specialWeaponType = WeaponType.bomb;
                specialWeapon = bomb;
                specialWeaponAmmoCount = 1;
                break;
        }
    }

    private void SetRapidFire(bool set)
    {
        rapidFireEnabled = set;
        if (set)
        {
            fireRate = FIRE_RATE_PERK;
            rapidFireTimer = RAPID_FIRE_TIMER;
        }
        else
        {
            fireRate = BASE_FIRE_RATE;
            rapidFireTimer = 0;
        }
    }

    /* handle state changes when picking up a powerup */
    public void PowerUpPickup(PowerUpType type)
    {
        switch(type){
            case PowerUpType.None:
                ammo = laser;
                SetSpecialWeapon(WeaponType.none);
                shielded = false;
                SetRapidFire(false);
                break;
            case PowerUpType.Missile:
                SetSpecialWeapon(WeaponType.missile);
                specialFireRate = MISSILE_FIRE_RATE;
                break;
            case PowerUpType.Bomb:
                SetSpecialWeapon(WeaponType.bomb);
                break;
            case PowerUpType.ExtraLife:
                gameParams.playerLives++;
                break;
            case PowerUpType.RapidFire:
                SetRapidFire(true);
                break;
            case PowerUpType.Shield:
                shielded = true;
                break;
        }
        
    }

}
