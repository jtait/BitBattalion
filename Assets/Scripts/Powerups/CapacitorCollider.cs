using UnityEngine;
using System.Collections;

public class CapacitorCollider : MonoBehaviour {

    void Start()
    {
        /* fix the rotation of the collider to be straight up after rotating capacitor model */
        Vector3 parentRotation = transform.parent.localEulerAngles;
        float parentXRot = parentRotation.x;
        float parentYRot = parentRotation.y;
        float xRot = 360 - parentXRot;
        float yRot = 180 - parentYRot;
        transform.localEulerAngles = new Vector3(xRot, yRot, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Weapon")
        {
            transform.GetComponentInParent<Capacitor>().SpawnPowerUp();
            Destroy(col.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }
}
