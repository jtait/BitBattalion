using UnityEngine;
using System.Collections;

public class BGSpawnAndMove : MonoBehaviour {

    private const float CAMERA_FRAME_HEIGHT = 24;
    private float startingYPosition;
    private float spawnMarker;
    private GameObject backgroundPrefab;
    private bool spawned = false;
    

	void Start () {
        startingYPosition = transform.position.y;
        backgroundPrefab = Resources.Load<GameObject>("Backgrounds/Moving_Panel");	
	}
	
	void Update () {

        if (transform.position.y < spawnMarker && !spawned)
        {
            spawned = true;
            GameObject clone = Instantiate(backgroundPrefab, transform.position + new Vector3(0, CAMERA_FRAME_HEIGHT, 0), Quaternion.identity) as GameObject;
            clone.transform.parent = transform.parent;
            clone.transform.localScale = Vector3.one;
        }
	}
}
