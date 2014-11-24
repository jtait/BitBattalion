using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject enemyToSpawn; // the bit prefab to spawn
    public bool destroyOnCompletion = true; // is the spawner destroyed when spawning is completed?
    public bool spawn = false; // is the spawner active?
    private bool spawnComplete = false; // has the spawn operation completed?
    
    public int amountToSpawn = 0; // the number of prefabs to spawn
    private int spawnedSoFar = 0; // the number of prefabs spawned so far
    
    public float spawnFrequency = 0; // how many seconds are between spawns
    private float timer; // the timer for the spawner
    public float initialWaitTime = 0; // if spawning doesn't start right away, how long does the spawner wait?

    public enum SpawnType {Normal = 0, Reverse = 1, Alternating = 2, Random = 3};
    public SpawnType spawnType; // the spawn setting for this spawner

    protected bool specialSet = false; // a special bool to alter the spawned bit

    private Transform playerTransformLocation; // the transform of the player
    private float yPosition; // the y position of this spawner
    private float yOffset; // the distance to the player when activated

    public bool bossAreaSpawner = false;

	void Start ()
    {
        playerTransformLocation = GameObject.Find("Player").transform;
        timer = initialWaitTime;
        yPosition = transform.position.y;
        if (bossAreaSpawner) yOffset = 0;
        else yOffset = 30;
	}

    void Update()
    {
        if (!spawnComplete)
        {

            if (spawn)
            {
                Spawn();
            }

            else if (playerTransformLocation.position.y + yOffset > yPosition)
            {
                spawn = true;
            }
        }
    }

    void Spawn()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            GameObject clone = GameObject.Instantiate(enemyToSpawn, transform.position, Quaternion.identity) as GameObject;

            if (spawnType == SpawnType.Reverse)
            {
                clone.GetComponent<GenericEnemy>().special = true;
            }

            if (spawnType == SpawnType.Alternating)
            {
                clone.GetComponent<GenericEnemy>().special = specialSet;
                specialSet = !specialSet;
            }

            if (spawnType == SpawnType.Random)
            {
                int rand = Random.Range(0, 2);
                if (rand == 1) clone.GetComponent<GenericEnemy>().special = true;
            }

            timer = spawnFrequency;
            spawnedSoFar++;
        }

        if (spawnedSoFar >= amountToSpawn && !bossAreaSpawner)
        {
            if (destroyOnCompletion)
            {
                Destroy(gameObject);
            }
            else
            {
                spawn = false;
                spawnComplete = true;
            }
        }

    }

    /* initialize parameters in spawner, used for endless mode */
    public void initForEndless(GameObject enemyToSpawn, int amount, float spawnFrequency, SpawnType mode)
    {
        this.enemyToSpawn = enemyToSpawn;
        this.destroyOnCompletion = true;
        this.spawn = true;
        this.amountToSpawn = amount;
        this.spawnFrequency = spawnFrequency;
        this.spawnType = mode;
    }


}
