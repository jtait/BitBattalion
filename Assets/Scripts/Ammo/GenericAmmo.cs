using UnityEngine;
using System.Collections;

public abstract class GenericAmmo : MonoBehaviour {

    public Vector3 shotVelocity; // the velocity of the projectile
    public float baseSpeed; // the baseSpeed of the projectile (allows velocity to have modifiers)
    protected float destructionTime; // the actual time to live
    public float timeToLive; // the unmodified time to live
    public int shotDamage; // the damage the shot does

	protected virtual void Start ()
    {
        destructionTime = Time.time + timeToLive;
	}
	
	protected virtual void FixedUpdate ()
    {
        GetComponent<Rigidbody>().velocity = shotVelocity;

        if (Time.time > destructionTime)
        {
            Destroy(gameObject);
        }
	}

    protected virtual void Update()
    {

    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "PlayerCollider") Destroy(gameObject);
    }

}
