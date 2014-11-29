using UnityEngine;
using System.Collections;

public class DestroyAmmo : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Weapon" || col.collider.tag == "EnemyWeapon")
        {
            Destroy(col.collider.gameObject);
        }
    }
}
