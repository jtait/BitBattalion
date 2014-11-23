using UnityEngine;
using System.Collections;

public class Bomb : GenericAmmo {

    // persistent game parameters
    private GameParameters gParams;

    /* sounds */
    AudioClip bombSound;

    void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        bombSound = Resources.Load<AudioClip>("SoundFX/bomb/103014__zgump__club-kick-0711");
    }

	protected override void Start () {
        base.Start();
        shotVelocity = Vector3.up * baseSpeed;
        rigidbody.AddForce(shotVelocity);
	}

    protected override void FixedUpdate()
    {
        
        if (Time.time > destructionTime)
        {
            Explode();
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
        AudioSource.PlayClipAtPoint(bombSound, transform.position, 1f);
        gParams.DestroyAllEnemiesInList();
        Destroy(gameObject);
    }
}
