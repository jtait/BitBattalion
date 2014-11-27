using UnityEngine;
using System.Collections;

public abstract class GenericBoss : MonoBehaviour {

    protected int health;
    protected GameParameters gParams;
    protected const float WAIT_TIME_FOR_LEVEL_LOAD = 4;
    protected Transform playerTransform; // the transform of the player
    protected int difficulty;
    protected bool bossActive = false; // is the boss active?
    protected float activateDistance; // the proximity of the player before becoming active
    protected int points; // the number of points awarded for killing the boss

    protected virtual void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        difficulty = gParams.difficulty;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {

    }
    
	protected virtual void Update ()
    {
        DeathCheck();
        if (playerTransform.position.y > transform.position.y - activateDistance)
        {
            bossActive = true;
        }
        else if (playerTransform.position.y < transform.position.y - activateDistance)
        {
            bossActive = false;
        }
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
        gParams.UpdateScore(points);
        DestroyAllEnemiesAndSpawners();

    }

    protected IEnumerator WaitForLevelLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // load next level here
    }

    /* called if the player loses the fight */
    public virtual void LoseSequence()
    {
        playerTransform.GetComponent<PlayerControl>().PlayerDeath();
    }

    protected void DestroyAllEnemiesAndSpawners()
    {
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("Enemy"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("Spawner"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("EnemyWeapon"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("BossMinion"));   
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
