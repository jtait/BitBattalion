using UnityEngine;
using System.Collections;

public class Turret : GenericEnemy {

    private const float BASE_FIRE_RATE = 20f; // the base fire rate of the ship.  Lower is faster.

    private Transform target;
    private float nextShot;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ammunition = Resources.Load<GameObject>("Ammo/EnemyLaser");
        nextShot = 0f;
        fireRate = BASE_FIRE_RATE * difficulty * 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
	
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
            Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);


            GameObject clone = GameObject.Instantiate(ammunition, launchFrom, Quaternion.identity) as GameObject;
            clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
            nextShot = Time.time + fireRate;
        }
    }

}
