using UnityEngine;
using System.Collections;

public class BitSpawner : MonoBehaviour {

    public GameObject bitToSpawn; // the bit prefab to spawn
    public bool destroyOnCompletion = true; // is the spawner destroyed when spawning is completed?
    public bool spawn = false; // is the spawner active?
    
    public int amountToSpawn = 0; // the number of prefabs to spawn
    private int spawnedSoFar = 0; // the number of prefabs spawned so far
    
    public float spawnFrequency = 0; // how many seconds are between spawns
    private float timer; // the timer for the spawner
    public bool spawnRightAway = true; // does spawning start when the spawner is created?
    public float initialWaitTime = 0; // if spawning doesn't start right away, how long does the spawner wait?

    public enum SpawnType {Normal, Alternating, Random};
    public SpawnType spawnType;

    private bool specialSet = false;

	void Start ()
    {
        if (spawnRightAway)
        {
            timer = 0;
        }
        else
        {
            timer = initialWaitTime;
        }
	}

    void FixedUpdate()
    {
        if (spawn)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                GameObject clone = GameObject.Instantiate(bitToSpawn, transform.position, Quaternion.identity) as GameObject;

                if (spawnType == SpawnType.Alternating){
                    clone.GetComponent<BitShip>().special = specialSet;
                    specialSet = !specialSet;
                }

                if (spawnType == SpawnType.Random)
                {
                    int rand = Random.Range(0, 2);
                    if(rand == 1) clone.GetComponent<BitShip>().special = true;
                }
                
                timer = spawnFrequency;
                spawnedSoFar++;
            }
            if (spawnedSoFar >= amountToSpawn)
            {
                if (destroyOnCompletion)
                {
                    Destroy(gameObject);
                }
                else
                {
                    spawn = false;
                }
            }
        }
    }

}
