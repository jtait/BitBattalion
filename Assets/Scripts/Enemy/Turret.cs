using UnityEngine;
using System.Collections;

public class Turret : GenericEnemy {

    private const float BASE_FIRE_RATE = 1f; // the base fire rate of the turret.  Lower is faster.
    private const float OFFSET_FROM_CENTER = 2f; // the offset so the turret shoots from the end of the barrel instead of the center of the transform

    private Transform target;
    private float nextShot;
    public float baseFireRate = 1;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ammunition = Resources.Load<GameObject>("Ammo/EnemyLaser");
        nextShot = 0f;
        fireRate = BASE_FIRE_RATE * baseFireRate;
	}
	
    void FixedUpdate()
    {
        Move();
        Shoot();
    }

    protected override void Move()
    {
        transform.LookAt(target, Vector3.back);
    }


    /* basic shoot function - spawns new projectile */
    protected override void Shoot()
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

}
