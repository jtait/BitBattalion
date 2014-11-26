using UnityEngine;
using System.Collections;

public class Turret_1 : GenericEnemy
{
    private const float OFFSET_FROM_CENTER = 1.5f; // the offset so the turret shoots from the end of the barrel instead of the center of the transform
    private const float MAX_START_DELAY = 5f;
    private const int NUMBER_OF_PROJECTILES = 6; // the number of projectiles to shoot each time
    private const int DEGREES_IN_CIRCLE = 360; // the number of degrees in a circle
    private const float BASE_SHOT_TTL = 4f; // the base time-to-live for the ammo the turret fires
    private const float BASE_SHOT_VELOCITY = 8f; // the base time-to-live for the ammo the turret fires
    private const float MIN_FIRE_DELAY = 3;
    private const float MAX_FIRE_DELAY = 4;

    private float nextShot; // keeps track of the next shot time
    private float angleOfRotation; // the angle of rotation between shots
    
    protected override void Start()
    {
        ammunition = Resources.Load<GameObject>("Ammo/EnemyLaser");
        nextShot = StartOffset();
        fireRate = StartFireRate();
        angleOfRotation = DEGREES_IN_CIRCLE / NUMBER_OF_PROJECTILES;
    }

    void FixedUpdate()
    {
        Shoot();
    }

    /* basic shoot function - spawns new projectiles in 6 directions */
    void Shoot()
    {
        if (Time.time > nextShot)
        {
            /* generate new objects to fire, instantiate with velocity, power, etc. */
            for (int i = 0; i < NUMBER_OF_PROJECTILES; i++)
            {
                Vector3 launchFrom = transform.position + (transform.right * OFFSET_FROM_CENTER);
                GameObject clone = GameObject.Instantiate(ammunition, launchFrom, Quaternion.identity) as GameObject;
                GenericAmmo ammo = clone.GetComponent<GenericAmmo>();
                // good from here down
                ammo.shotVelocity = (ammo.transform.position - transform.position) * ammo.baseSpeed * 0.5f * difficulty * BASE_SHOT_VELOCITY;
                ammo.timeToLive = ammo.timeToLive * 0.5f * difficulty + BASE_SHOT_TTL;
                transform.RotateAround(transform.position, Vector3.forward, angleOfRotation); // actually rotates the transform around its center
            }
            nextShot = Time.time + fireRate;
        }
    }

    /* overriden in case the turrets are spawned in endless mode
    protected override void Death()
    {
        if (health <= 0)
        {
            if (gParams.endlessMode)
            {
                gParams.endlessModeTurrets--;
            }
            gParams.UpdateScore(pointValue);
            Destroy(gameObject);
        }
    }

    /* used to set the time until the first shot */
    private float StartOffset()
    {
        return Random.Range(0, MAX_START_DELAY);
    }

    /* used to set the fire rate */
    private float StartFireRate()
    {
        return Random.Range(MIN_FIRE_DELAY, MAX_FIRE_DELAY);
    }

}
