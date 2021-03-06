﻿using UnityEngine;
using System.Collections;

public class Turret_0 : GenericEnemy, IGenericEnemy {

    private const float MIN_FIRE_RATE = 1f; // the base fire rate of the turret.  Lower is faster.
    private const float OFFSET_FROM_CENTER = 3.5f; // the offset so the turret shoots from the end of the barrel instead of the center of the transform
    private const float MAX_START_DELAY = 5f;
    private const float MIN_FIRE_DELAY = 1;
    private const float MAX_FIRE_DELAY = 3;
    private const float TURRET_AMMO_BASE_SPEED = 3f;

    private Transform target; // the transform of the target of the shot
    private float nextShot; // the time until the next shot

    protected override void Awake()
    {
        base.Awake();
        ammunition = Resources.Load<GameObject>("Ammo/EnemyLaser");
    }

	protected override void Start ()
    {
        base.Start();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        nextShot = Random.Range(0, MAX_START_DELAY);
        fireRate = Random.Range(MIN_FIRE_RATE + MIN_FIRE_DELAY, MAX_FIRE_DELAY);
	}
	
    void FixedUpdate()
    {
        Move();
        Shoot();
    }

    public void Move()
    {
        transform.LookAt(target, Vector3.back);
    }

    /* basic shoot function - spawns new projectile */
    public void Shoot()
    {
        if (Time.time > nextShot && Active())
        {
            // generate a new object to fire, instantiate with velocity, power, etc.
            Vector3 launchFrom = transform.position + transform.forward * OFFSET_FROM_CENTER;
            GameObject clone = GameObject.Instantiate(ammunition, launchFrom, Quaternion.identity) as GameObject;
            GenericAmmo ammo = clone.GetComponent<GenericAmmo>();
            ammo.shotVelocity = (target.position - transform.position).normalized * TURRET_AMMO_BASE_SPEED * gParams.difficulty;
            ammo.timeToLive = ammo.timeToLive * gParams.difficulty;
            nextShot = Time.time + fireRate;
        }
    }

    public bool Active()
    {
        return onScreen;
    }

    public bool Enabled()
    {
        return true;
    }

    public new void Death()
    {
        if (gParams.endlessMode)
        {
            gParams.endlessModeTurrets--;
        }
        base.Death();
    }
}
