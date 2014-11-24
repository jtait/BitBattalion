using UnityEngine;
using System.Collections;

/* player laser weapon */
public class PlayerLaser : GenericAmmo {

    protected override void Start()
    {
        base.Start();
        shotVelocity = Vector3.up * baseSpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Enemy" && col.collider.name != "LaserShot") Destroy(gameObject); // destroy self if it collides with anything except enemy or another laser
    }

}
