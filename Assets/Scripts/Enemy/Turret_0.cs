using UnityEngine;
using System.Collections;

public class Turret_0 : GenericEnemy {

    private const float MIN_FIRE_RATE = 1f; // the base fire rate of the turret.  Lower is faster.
    private const float OFFSET_FROM_CENTER = 3.7f; // the offset so the turret shoots from the end of the barrel instead of the center of the transform
    private const float MAX_START_DELAY = 5f;

    private Transform target;
    private float nextShot;
    public float minFireDelay = 1;
    public float maxFireDelay = 3;

	// Use this for initialization
	protected override void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ammunition = Resources.Load<GameObject>("Ammo/EnemyLaser");
        nextShot = StartOffset();
        fireRate = StartFireRate();
	}
	
    void FixedUpdate()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        transform.LookAt(target, Vector3.back);
    }


    /* basic shoot function - spawns new projectile */
    void Shoot()
    {
        if (Time.time > nextShot)
        {

            // generate a new object to fire, instantiate with velocity, power, etc.
            Vector3 launchFrom = transform.position + transform.forward * OFFSET_FROM_CENTER;// new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);


            GameObject clone = GameObject.Instantiate(ammunition, launchFrom, Quaternion.identity) as GameObject;

            GenericAmmo ammo = clone.GetComponent<GenericAmmo>();

            ammo.shotVelocity = (target.position - transform.position) * ammo.baseSpeed * 0.5f * difficulty;
            ammo.timeToLive = ammo.timeToLive * 0.5f * difficulty;
            nextShot = Time.time + fireRate;
        }
    }

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

    private float StartOffset()
    {
        return Random.Range(0, MAX_START_DELAY);
    }

    private float StartFireRate()
    {
        return Random.Range(MIN_FIRE_RATE + minFireDelay, maxFireDelay);
    }

}
