using UnityEngine;
using System.Collections;

public class TurretAmmo : GenericAmmo {

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
        if(col.collider.tag != "Player") Destroy(gameObject);
    }
}
