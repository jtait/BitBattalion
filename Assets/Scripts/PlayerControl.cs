using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    // movement related variables
    public float strafeForce = 10f;
    private float strafeDirection;
    private float velocityDirection;
    public float velocityAddition = 2f;
    
    void Start()
    {

    }

    void FixedUpdate()
    {

        // strafing
        strafeDirection = Input.GetAxis("Horizontal");
        rigidbody.AddForce(new Vector3(strafeDirection * strafeForce, 0, 0));

        // forward / backward movement
        velocityDirection = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector3(0, velocityAddition * velocityDirection, 0);

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Enemy")
        {
            PlayerDeath();
        }
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

    void PlayerDeath()
    {
        /* destroy game object */

        /* move player to last checkpoint */

        /* recreate player ship */

        print("died");
    }

}
