using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

    public PowerUpType type;

    void Start()
    {
        /* flip the transform over */
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerCollider")
        {
            col.GetComponentInParent<PlayerControl>().PowerUpPickup(type); // trigger the powerup method on the player to assign a powerup
            Destroy(gameObject); // destroy the powerup object
        }
    }

}
