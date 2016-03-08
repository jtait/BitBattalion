using UnityEngine;
using System.Collections;

/* player laser weapon */
public class PlayerLaser : GenericAmmo {

    protected override void Start()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        localVelocity.y = baseSpeed;
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(localVelocity);
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Enemy")
        {
            Destroy(gameObject); // destroy self if it collides with anything except an enemy
        }
    }

}
