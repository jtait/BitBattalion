using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EnemyLaser : GenericAmmo {

    AudioClip laserSound;
    private const float soundVolume = 0.05f;

    void Awake()
    {
        laserSound = Resources.Load<AudioClip>("SoundFX/enemyLaser/35683__jobro__laser6");
    }

    protected override void Start()
    {
        base.Start();
        AudioSource.PlayClipAtPoint(laserSound, transform.position, soundVolume);
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
