using UnityEngine;
using System.Collections;

public class EnemyLaser : GenericAmmo {

    AudioClip pewpew;

    void Awake()
    {
        pewpew = Resources.Load<AudioClip>("SoundFX/enemyLaser/35683__jobro__laser6");
    }

    protected override void Start()
    {
        base.Start();
        AudioSource.PlayClipAtPoint(pewpew, transform.position, 1f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "PlayerCollider" && col.collider.name != "EnemyLaser") Destroy(gameObject);
    }
}
