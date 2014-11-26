using UnityEngine;
using System.Collections;
using System;

public class SpawnerSpawner : MonoBehaviour {

    private const int NUMBER_OF_LOCATIONS = 3; // the number of spawner locations in the world
    private const int SPAWNER_OFFSET_FROM_CENTER = 10; // the offset from the center for the spawner positions
    private const float INITIAL_SPAWN_FREQUENCY = 5; // the starting spawn frequency in seconds
    private int NUMBER_OF_COMBINATIONS = (int) Mathf.Pow(2, NUMBER_OF_LOCATIONS); // the number of possible position combinations

    private Vector3[] turretPositionOffset; // the offset to position the turret on the screen
    private bool spawningTurrets = false;

    private Vector3[] spawnerLocations; // possible locations for spawners

    private float spawnFrequency; // how many seconds are between spawns
    private float spawnTimer; // the timer for the spawner
    private int spawnedSoFar = 0; // counter for the wave number

    private GameObject bit_0, byte_0, turret_0; // references to possible enemies to spawn
    
    GameParameters gParams; // reference to the global game parameters
    GameObject genericSpawner; // the spawner to create
    Spawner spawnerParams; // the spawner parameters

    private static Array possibleSpawnTypes; // the special enumerator for the spawner
    private static int numberOfSpawnTypes;
    
    void Awake()
    {
        spawnFrequency = INITIAL_SPAWN_FREQUENCY;
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        gParams.endlessMode = true;
        possibleSpawnTypes = Enum.GetValues(typeof(Spawner.SpawnType));
        numberOfSpawnTypes = possibleSpawnTypes.Length;

        /* load enemies into variables */
        bit_0 = Resources.Load<GameObject>("Enemies/BitShip_0");
        byte_0 = Resources.Load<GameObject>("Enemies/ByteShip_0");
        turret_0 = Resources.Load<GameObject>("Enemies/Turret_0");

        /* load generic spawner */
        genericSpawner = Resources.Load<GameObject>("Spawner");
        spawnerParams = genericSpawner.GetComponent<Spawner>();
        
        /* initialize array of locations */
        spawnerLocations = new Vector3[NUMBER_OF_LOCATIONS];
        turretPositionOffset = new Vector3[NUMBER_OF_LOCATIONS];
        spawnerLocations[0] = new Vector3(-SPAWNER_OFFSET_FROM_CENTER, 0, 0) + transform.position;
        turretPositionOffset[0] = new Vector3(-2f, -4f, 0);
        spawnerLocations[1] = transform.position;
        turretPositionOffset[1] = new Vector3(0, -8f, 0);
        spawnerLocations[2] = new Vector3(SPAWNER_OFFSET_FROM_CENTER, 0, 0) + transform.position;
        turretPositionOffset[2] = new Vector3(2f, -4f, 0);
        
    }

	void Update () {
        Spawn();
	}

    private void SpawnTurrets(GameObject[] set)
    {
        for (int i = 0; i < NUMBER_OF_LOCATIONS; i++)
        {
            if (set[i] != null)
            {
                GameObject.Instantiate(set[i], spawnerLocations[i] + turretPositionOffset[i], Quaternion.identity);
                gParams.endlessModeTurrets++;
            }
        }
    }

    private void SpawnShips(GameObject[] set)
    {
        for (int i = 0; i < NUMBER_OF_LOCATIONS; i++)
        {
            if (set[i] != null)
            {
                GameObject.Instantiate(set[i], spawnerLocations[i], Quaternion.identity);
            }
        }
    }

    /* handles spawning */
    private void Spawn()
    {
        spawnTimer -= Time.deltaTime;

        /* if timer has expired, spawn spawners */
        if (spawnTimer < 0)
        {
            GameObject[] set = GenerateSpawnerSet();            

            if (spawningTurrets)
                SpawnTurrets(set);
            else
                SpawnShips(set);

            spawningTurrets = false;
            spawnTimer = spawnFrequency;
            spawnedSoFar++;
            if (spawnedSoFar > 10)
            {
                spawnedSoFar = 0; // reset counter
                gParams.difficulty++; // increase difficulty
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
        GameObject[] set = new GameObject[NUMBER_OF_LOCATIONS + 1]; // the empty set of spawners
        
        GameObject enemyToSpawn = RandomEnemy(); // the enemy to spawn

        if (enemyToSpawn == byte_0)
        {
            spawnerParams.initForEndless(byte_0, 1, 1, Spawner.SpawnType.Normal);
        }
        else if (enemyToSpawn == turret_0)
        {
            spawningTurrets = true;
            spawnerParams.initForEndless(turret_0, 1, 1, Spawner.SpawnType.Normal);
        }
        else
        {
            int type = UnityEngine.Random.Range(0, numberOfSpawnTypes);
            spawnerParams.initForEndless(bit_0, 5, 1, (Spawner.SpawnType)possibleSpawnTypes.GetValue(type));
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
        int random = UnityEngine.Random.Range(0, 6);
        if (random < 4)
            return bit_0;
        if (random < 5 && gParams.endlessModeTurrets <= 0)
            return turret_0;
        else
            return byte_0;
    }

}
