using UnityEngine;
using System.Collections;

public class GenericAmmo : MonoBehaviour {

    public Vector3 shotVelocity;
    public float baseSpeed;
    private float destructionTime;
    public float timeToLive;
    public int shotDamage;

	// Use this for initialization
	void Start () {
        shotVelocity = Vector3.up * baseSpeed;
        destructionTime = Time.time + timeToLive;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidbody.velocity = shotVelocity;

        if (Time.time > destructionTime)
        {
            Destroy(gameObject);
        }



	}

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag != "Weapon") Destroy(gameObject);
    }
}
