using UnityEngine;
using System.Collections;

public class Bomb : GenericAmmo {

    // persistent game parameters
    private GameParameters gParams;

    void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
    }

	protected override void Start () {
        base.Start();
        shotVelocity = Vector3.up * baseSpeed;
        rigidbody.AddForce(shotVelocity);
	}

    protected override void FixedUpdate()
    {

        //rigidbody.velocity = shotVelocity;

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

    /* explode and destroy all enemies on screen */
    private void Explode()
    {
        gParams.DestroyAllEnemiesInList();
    }
}
