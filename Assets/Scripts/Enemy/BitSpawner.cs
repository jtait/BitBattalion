﻿using UnityEngine;
using System.Collections;

public class BitSpawner : MonoBehaviour {

    public GameObject bitToSpawn; // the bit prefab to spawn
    public bool destroyOnCompletion = true; // is the spawner destroyed when spawning is completed?
    public bool spawn = false; // is the spawner active?
    private bool spawnComplete = false; // has the spawn operation completed? 
    
    public int amountToSpawn = 0; // the number of prefabs to spawn
    private int spawnedSoFar = 0; // the number of prefabs spawned so far
    
    public float spawnFrequency = 0; // how many seconds are between spawns
    private float timer; // the timer for the spawner
    public float initialWaitTime = 0; // if spawning doesn't start right away, how long does the spawner wait?

    public enum SpawnType {Normal, Reverse, Alternating, Random};
    public SpawnType spawnType; // the spawn setting for this spawner

    private bool specialSet = false; // a special bool to alter the spawned bit

    private Transform lookAheadTrigger; // the transform of the look ahead trigger attached to the player
    private float yPosition; // the y position of this spawner
    private float yOffset = 30;

	void Start ()
    {
        lookAheadTrigger = GameObject.Find("Player").transform;
        timer = initialWaitTime;
        yPosition = transform.position.y;
	}

    void FixedUpdate()
    {
        if (!spawnComplete)
        {

            if (spawn)
            {
                Spawn();
            }

            else if (lookAheadTrigger.position.y + yOffset > yPosition)
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
            GameObject clone = GameObject.Instantiate(bitToSpawn, transform.position, Quaternion.identity) as GameObject;

            if (spawnType == SpawnType.Reverse)
            {
                clone.GetComponent<BitShip>().special = true;
            }

            if (spawnType == SpawnType.Alternating)
            {
                clone.GetComponent<BitShip>().special = specialSet;
                specialSet = !specialSet;
            }

            if (spawnType == SpawnType.Random)
            {
                int rand = Random.Range(0, 2);
                if (rand == 1) clone.GetComponent<BitShip>().special = true;
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
                spawnComplete = true;
            }
        }

    }


}