using UnityEngine;
using System.Collections;

public class PlayerScatterLaser: MonoBehaviour {

    private const int NUMBER_OF_PROJECTILES = 5; // must be an odd number
    private const float OFFSET_FROM_CENTER = 1.0f; // the offset from the center of the (ship's) transform
    private const float SPREAD_ANGLE = 20f; // the angle between individual lasers

    private GameObject ammunition;

    void Awake()
    {
        ammunition = Resources.Load<GameObject>("Ammo/LaserShot");
    }

    void Start()
    {
        /* generate new objects to fire */

        for (int i = -(NUMBER_OF_PROJECTILES / 2); i < (NUMBER_OF_PROJECTILES / 2) + 1; i++)
        {
            Vector3 launchFrom = transform.position + new Vector3(0, OFFSET_FROM_CENTER, 0);
            GameObject laser = GameObject.Instantiate(ammunition, launchFrom, Quaternion.identity) as GameObject;
            laser.transform.RotateAround(transform.position, Vector3.forward, SPREAD_ANGLE * i);
        }

        /* after launching, destroy and free memory */
        Destroy(gameObject);

    }

}
