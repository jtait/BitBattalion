using UnityEngine;
using System.Collections;

public class Bomb : GenericAmmo {

	// Use this for initialization
	void Start () {
        shotVelocity = Vector3.up * baseSpeed;
        rigidbody.velocity = shotVelocity;
	}

    protected override void FixedUpdate()
    {
        
        if (Time.time > destructionTime)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter()
    {
        /* stop the bomb if it hits a collider */
        rigidbody.velocity = Vector3.zero;
    }

    private void Explode()
    {

    }
}
