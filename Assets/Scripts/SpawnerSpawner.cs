using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerSpawner : MonoBehaviour {

    private GameObject[] spawners; // spawners
    private List<GameObject>[] arrayOfLists;
    private Vector3[] spawnerLocations;

    void Awake()
    {
        GameObject genericSpawner = Resources.Load<GameObject>("Spawner");

    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
