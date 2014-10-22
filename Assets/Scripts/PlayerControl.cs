using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    // movement related variables
    public float strafeForce = 10f;
    private float strafeDirection;
    public float forwardVelocity = 5f;
    private float velocityDirection;
    public float velocityAddition = 2f;
    
    void Start()
    {
        collider.enabled = false;
    }

    void FixedUpdate()
    {

        // strafing
        strafeDirection = Input.GetAxis("Horizontal");
        rigidbody.AddForce(new Vector3((strafeDirection * strafeForce), 0, 0));

        // forward / backward movement
        velocityDirection = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector3(0, forwardVelocity + velocityAddition * velocityDirection, 0);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    // create and launch projectile from ships location
    void Shoot()
    {
        // generate a new object to fire, instantiate with velocity, power, etc.
    }

}
