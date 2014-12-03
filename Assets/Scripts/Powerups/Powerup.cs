using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

    public PowerUpType type;
    private AudioClip pickupSound;

    void Awake()
    {
        pickupSound = Resources.Load<AudioClip>("SoundFX/powerupPickup/59473__glewlio__melody-sound-15");
    }

    void Start()
    {
        /* flip the transform over */
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerCollider")
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.75f); // play explosion sound
            col.GetComponentInParent<PlayerControl>().PowerUpPickup(type); // trigger the powerup method on the player to assign a powerup
            Destroy(gameObject); // destroy the powerup object
        }
    }

}
