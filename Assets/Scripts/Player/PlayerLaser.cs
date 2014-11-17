using UnityEngine;
using System.Collections;

public class PlayerLaser : GenericAmmo {

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Enemy") Destroy(gameObject);
    }

}
