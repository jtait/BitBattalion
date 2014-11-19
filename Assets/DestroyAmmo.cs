using UnityEngine;
using System.Collections;

public class DestroyAmmo : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Weapon")
        {
            Destroy(col.gameObject);
        }
    }
}
