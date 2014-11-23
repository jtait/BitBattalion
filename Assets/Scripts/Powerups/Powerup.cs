using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

    public PowerUpType type;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponentInParent<PlayerControl>().PowerUpPickup(type); // trigger the powerup method on the player to assign a powerup
            Destroy(gameObject); // destroy the powerup object
        }
    }

}
