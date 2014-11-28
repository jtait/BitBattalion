using UnityEngine;
using System.Collections;

public class Destruction : MonoBehaviour {

    /* destroy any collider that enters the trigger */
    void OnCollisionEnter(Collision col){
        Destroy(col.collider.gameObject);
    }

    /* destroy any trigger that enters the trigger */
    void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
    }
}
