using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

    private PowerUpType type;

	// Use this for initialization
	void Start () {
        type = RandomPowerUp();
	}
	
    private PowerUpType RandomPowerUp()
    {
        int slotMachine = Random.Range(0, 58);
        if (slotMachine < 15) return PowerUpType.RapidFire;
        if (slotMachine < 30) return PowerUpType.Shield;
        if (slotMachine < 45) return PowerUpType.Missile;
        if (slotMachine < 55) return PowerUpType.Bomb;
        if (slotMachine < 57) return PowerUpType.ExtraLife;
        else return PowerUpType.None;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerControl>().PowerUpPickup(type);
        }
    }
}
