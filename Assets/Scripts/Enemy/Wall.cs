using UnityEngine;
using System.Collections;

public class Wall : GenericEnemy{

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Weapon")
        {
            Destroy(col.collider.gameObject); // destroy the weapon that hit the enemy
        }
    }
}
