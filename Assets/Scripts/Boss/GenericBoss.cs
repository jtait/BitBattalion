using UnityEngine;
using System.Collections;

public abstract class GenericBoss : MonoBehaviour {

    protected int health;
    protected GameParameters gParams;
    protected const float WAIT_TIME_FOR_LEVEL_LOAD = 4;
    protected Transform playerTransform; // the transform of the player
    protected int difficulty;
    protected bool bossActive = false; // is the boss active?

    void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        difficulty = gParams.difficulty;
        playerTransform = GameObject.Find("Player").transform;
    }

	protected virtual void Start ()
    {
	
	}
	
	protected virtual void Update ()
    {
        DeathCheck();
	}

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Weapon")
        {
            health--;
        }
    }

    protected virtual void DeathCheck()
    {
        if (health <= 0)
        {
            DeathSequence();
        }
    }

    /* called if the player wins and the boss dies */
    protected virtual void DeathSequence()
    {
        /* override in child - call base at the begining of the death sequence */
        StartCoroutine(WaitForLevelLoad(WAIT_TIME_FOR_LEVEL_LOAD));
        renderer.active = false;
        collider.enabled = false;
        DestroyAllEnemiesAndSpawners();

    }

    protected IEnumerator WaitForLevelLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // load next level here
    }

    /* called if the player loses the fight */
    protected virtual void LoseSequence()
    {
        playerTransform.GetComponentInChildren<PlayerControl>().PlayerDeath();
    }

    protected void DestroyAllEnemiesAndSpawners()
    {
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("Enemy"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("Spawner"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("EnemyWeapon"));        
    }

    private void DestroyGameObjectsInArray(GameObject[] array)
    {
        foreach (GameObject enemy in array)
        {
            if (enemy != null)
                Destroy(enemy);
        }
    }

}
