using UnityEngine;
using System.Collections;

public class EnemyLaser : GenericAmmo {

    protected override void Start()
    {
        base.Start();
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
