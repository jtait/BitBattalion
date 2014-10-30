using UnityEngine;
using System.Collections;

public class BitSpawner : MonoBehaviour {

    public Transform bitToSpawn; // the bit prefab to spawn
    public bool destroyOnCompletion = true; // is the spawner destroyed when spawning is completed?
    public bool spawn = false; // is the spawner active?
    
    public int amountToSpawn = 0; // the number of prefabs to spawn
    private int spawnedSoFar = 0; // the number of prefabs spawned so far
    
    public float spawnFrequency = 0; // how many seconds are between spawns
    private float timer; // the timer for the spawner
    public bool spawnRightAway = true; // does spawning start when the spawner is created?
    public float initialWaitTime = 0; // if spawning doesn't start right away, how long does the spawner wait?

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
                GameObject.Instantiate(bitToSpawn, transform.position, Quaternion.identity);
                timer = spawnFrequency;
                spawnedSoFar++;
            }
            if (spawnedSoFar >= amountToSpawn)
            {
                if (destroyOnCompletion)
                {
                    Destroy(this);
                }
                else
                {
                    spawn = false;
                }
            }
        }
    }
}
