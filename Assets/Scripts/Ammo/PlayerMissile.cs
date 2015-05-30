using UnityEngine;
using System.Collections;

public class PlayerMissile : GenericAmmo {

    private const int NUMBER_OF_PROJECTILES = 5; // must be an odd number
    private const float SPAWNED_SHOT_TTL = 3;
    private const int DEGREES_IN_CIRCLE = 360; // the number of degrees in a circle
    private const float OFFSET_FROM_CENTER = 1.5f; // the offset from the center of the transform

    private GameObject ammunition;

    void Awake()
    {
        ammunition = Resources.Load<GameObject>("Ammo/LaserShot");
    }

    protected override void Start()
    {
        timeToLive = 0.05f;
        destructionTime = Time.time + timeToLive;
        shotVelocity = Vector3.up * baseSpeed;
    }

    protected override void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = shotVelocity;

        if (Time.time > destructionTime)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            /* generate new objects to fire, instantiate with velocity, power, etc. */
            for (int i = (-NUMBER_OF_PROJECTILES / 2); i < (NUMBER_OF_PROJECTILES / 2) + 1; i++)
            {
                Vector3 launchFrom = transform.position + (transform.right * OFFSET_FROM_CENTER * i);
                GameObject clone = GameObject.Instantiate(ammunition, launchFrom, Quaternion.identity) as GameObject;
                GenericAmmo ammo = clone.GetComponent<GenericAmmo>();
                ammo.shotVelocity = (transform.position + Vector3.forward) * ammo.baseSpeed * 0.5f;
                ammo.timeToLive = ammo.timeToLive * 0.5f + SPAWNED_SHOT_TTL;
            }
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Enemy" && col.collider.name != "LaserShot") Destroy(gameObject); // destroy self if it collides with anything except enemy or another laser
    }
}
