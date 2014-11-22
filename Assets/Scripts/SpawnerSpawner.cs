using UnityEngine;
using System.Collections;
using System;

public class SpawnerSpawner : MonoBehaviour {

    private const int NUMBER_OF_LOCATIONS = 3; // the number of spawner locations in the world
    private const int SPAWNER_OFFSET_FROM_CENTER = 10; // the offset from the center for the spawner positions
    private const float INITIAL_SPAWN_FREQUENCY = 11;
    private const float DIFFICULTY_MULTIPLIER = 0.25f;
    private int NUMBER_OF_COMBINATIONS = (int) Mathf.Pow(2, NUMBER_OF_LOCATIONS); // the number of possible position combinations

    private GameObject[] spawners; // spawners
    private Vector3[] spawnerLocations; // possible locations for spawners

    private float spawnFrequency; // how many seconds are between spawns
    private float timer; // the timer for the spawner

    private GameObject bit_0, byte_0;

    private int spawnedSoFar = 0;

    GameParameters gParams;

    GameObject[] nextSetToSpawn;

    GameObject genericSpawner;
    BitSpawner spawnerParams;

    private static Array possibleSpawnTypes;
    private static int numberOfSpawnTypes;
    
    void Awake()
    {
        spawnFrequency = INITIAL_SPAWN_FREQUENCY;
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        possibleSpawnTypes = Enum.GetValues(typeof(BitSpawner.SpawnType));
        numberOfSpawnTypes = possibleSpawnTypes.Length;

        /* load enemies into variables */
        bit_0 = Resources.Load<GameObject>("Enemies/BitShip_0");
        byte_0 = Resources.Load<GameObject>("Enemies/ByteShip_0");

        /* load generic spawner */
        genericSpawner = Resources.Load<GameObject>("Spawner");
        spawnerParams = genericSpawner.GetComponent<BitSpawner>();
        
        /* initialize array of locations */
        spawnerLocations = new Vector3[NUMBER_OF_LOCATIONS];
        spawnerLocations[0] = new Vector3(-SPAWNER_OFFSET_FROM_CENTER, 0, 0) + transform.position;
        spawnerLocations[1] = transform.position;
        spawnerLocations[2] = new Vector3(SPAWNER_OFFSET_FROM_CENTER, 0, 0) + transform.position;
        
    }

	void Update () {
        Spawn();
        print(gParams.difficulty);
	}

    /* handles spawning */
    private void Spawn()
    {
        timer -= Time.deltaTime;

        /* if timer has expired, spawn spawners */
        if (timer < 0)
        {
            GameObject[] set = GenerateSpawnerSet();

            for (int i = 0; i < NUMBER_OF_LOCATIONS; i++)
            {
                if(set[i] != null)
                    GameObject.Instantiate(set[i], spawnerLocations[i], Quaternion.identity);
            }

            timer = spawnFrequency;
            spawnedSoFar++;
            if (spawnedSoFar > 10)
            {
                spawnedSoFar = 0; // reset counter
                gParams.difficulty++; // increase difficulty
                spawnFrequency -= gParams.difficulty * DIFFICULTY_MULTIPLIER;
            }
        }

    }

    /* creates a binary style array of active positions, to use when generating spawner set */
    private int[] genarateActiveSpawnersForThisRound(){
        int pattern = UnityEngine.Random.Range(1, NUMBER_OF_COMBINATIONS);

        int[] positions = new int[NUMBER_OF_LOCATIONS];
        int index = 0;
        int remainder;
        while (pattern > 0)
        {
            remainder = pattern % 2;
            pattern /= 2;
            positions[index] = remainder;
            index++;
        }
        
        return positions;
    }

    /* generates a random positioning of a random spawner */
    private GameObject[] GenerateSpawnerSet()
    {
        GameObject[] set = new GameObject[NUMBER_OF_LOCATIONS]; // the empty set of spawners
        
        GameObject enemyToSpawn = RandomEnemy(); // the enemy to spawn
        if (enemyToSpawn == byte_0)
        {
            spawnerParams.initForEndless(byte_0, 1, 1, BitSpawner.SpawnType.Normal);
        }
        else
        {
            int type = UnityEngine.Random.Range(0, numberOfSpawnTypes);
            spawnerParams.initForEndless(bit_0, 5, 1, (BitSpawner.SpawnType)possibleSpawnTypes.GetValue(type));
        }
        
        int[] isActive = genarateActiveSpawnersForThisRound();

        /* if the position is active this round, return a spawner to place there */
        for (int i = 0; i < NUMBER_OF_LOCATIONS; i++)
        {
            if (isActive[i] == 1)
            {
                set[i] = genericSpawner;
            }
            else
                set[i] = null;
        }

        return set;
    }

    /* picks a random enemy to spawn */
    private GameObject RandomEnemy()
    {
        int random = UnityEngine.Random.Range(0, 5);
        if (random < 4)
            return bit_0;
        else
            return byte_0;
    }

}
