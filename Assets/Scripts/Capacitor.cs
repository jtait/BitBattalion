using UnityEngine;
using System.Collections;

public class Capacitor : MonoBehaviour
{
    private const float SPAWN_OFFSET = 10f;
    public bool randomType = true;
    public PowerUpType predeterminedType;

    private PowerUpType type;
    Vector3 spawnPosition;

    void Awake()
    {
        float[] rotations = RandomRotation();
        transform.localEulerAngles = new Vector3(rotations[0], rotations[1], 0);
    }

    // Use this for initialization
    void Start()
    {
        if (randomType) type = RandomPowerUp();
        else type = predeterminedType;
        spawnPosition = (transform.position + Vector3.back * SPAWN_OFFSET);
    }

    private float[] RandomRotation()
    {
        float xRot = 0;
        float yRot = 0;
        while (xRot == 0 && yRot == 0)
        {
            xRot = Random.Range(-15, 15);
            yRot = Random.Range(165, 195);
        }
        float[] rotations = {xRot, yRot};
        return rotations;
    }

    private PowerUpType RandomPowerUp()
    {
        int random = Random.Range(0, 58);
        if (random < 15) return PowerUpType.RapidFire;
        if (random < 30) return PowerUpType.Shield;
        if (random < 45) return PowerUpType.Missile;
        if (random < 55) return PowerUpType.Bomb;
        if (random < 57) return PowerUpType.ExtraLife;
        else return PowerUpType.None;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Weapon")
        {
            SpawnPowerUp(type);
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }

    private void SpawnPowerUp(PowerUpType type)
    {
        GameObject powerUp = GetPowerUp(type);
        if (powerUp != null)
        {
            GameObject clone = GameObject.Instantiate(powerUp, spawnPosition, Quaternion.identity) as GameObject;
            clone.GetComponent<Powerup>().type = type;
        }
    }

    private GameObject GetPowerUp(PowerUpType type)
    {
        GameObject powerUp = null;

        switch (type)
        {
            case PowerUpType.None:
                break;
            case PowerUpType.Missile:
                powerUp = Resources.Load<GameObject>("Powerups/PowerUp_Missile");
                break;
            case PowerUpType.Bomb:
                powerUp = Resources.Load<GameObject>("Powerups/PowerUp_Bomb");
                break;
            case PowerUpType.ExtraLife:
                powerUp = Resources.Load<GameObject>("Powerups/Life_0");
                break;
            case PowerUpType.RapidFire:
                powerUp = Resources.Load<GameObject>("Powerups/PowerUp_Rapid");
                break;
            case PowerUpType.Shield:
                powerUp = Resources.Load<GameObject>("Powerups/PowerUp_Shield");
                break;
            default:
                break;
        }

        return powerUp;

    }
}
