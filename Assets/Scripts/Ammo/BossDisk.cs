using UnityEngine;
using System.Collections;

public class BossDisk : GenericAmmo
{

    protected override void Start()
    {
        base.Start();
        /* flip the transform over */
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerCollider")
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<GenericBoss>().LoseSequence();
        }
    }
}
