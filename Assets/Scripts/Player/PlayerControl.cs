using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    // movement
    public float strafeForce = 10f;
    private float strafeDirection;
    private float velocityDirection;
    public float velocityAddition = 2f;

    // weapons
    public GameObject ammo;
    public float fireRate; // delay between shots
    private float nextShot;



    private GameParameters gameParams;

    void Awake()
    {
        nextShot = 0f;
        gameParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
    }
    

    void FixedUpdate()
    {

        // strafing
        strafeDirection = Input.GetAxis("Horizontal");
        rigidbody.AddForce(new Vector3(strafeDirection * strafeForce, 0, 0));

        // forward / backward movement
        velocityDirection = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector3(0, velocityAddition * velocityDirection, 0);

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Enemy")
        {
            PlayerDeath();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Shoot();
        }
    }

    // create and launch projectile from ships location
    void Shoot()
    {
        if (Time.time > nextShot)
        {
            // generate a new object to fire, instantiate with velocity, power, etc.
            Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject clone = GameObject.Instantiate(ammo, launchFrom, Quaternion.identity) as GameObject;
            clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
            nextShot = Time.time + fireRate;
        }
    }

    void PlayerDeath()
    {
        /* decrease player lives */
        gameParams.playerLives--;

        if (gameParams.playerLives > 0)
        {
            /* destroy all enemies */
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in toDestroy)
            {
                Destroy(enemy);
            }

            /* move player to last checkpoint */
            transform.parent.position = gameParams.lastCheckpoint;

        }
        else
        {
            /* GAME OVER */
        }
        


    }

    void OnTriggerEnter(Collider col)
    {
        /* update checkpoints as the player reaches them */
        if (col.tag == "CheckPoint")
        {
            gameParams.lastCheckpoint = col.transform.position;
        }
    }

}
