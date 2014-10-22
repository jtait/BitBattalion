using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    public float strafeForce = 10f;
    private float strafeDirection;

    public float forwardVelocity = 5f;
    private float velocityDirection;
    public float velocityAddition = 2f;
    
    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

    }

    //
    void Shoot()
    {

    }

}
