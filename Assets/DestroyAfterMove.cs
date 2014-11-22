using UnityEngine;
using System.Collections;

public class DestroyAfterMove : MonoBehaviour {

    private const float CAMERA_FRAME_HEIGHT = 24;
    private float startingYPosition;
    private float destroyMarker;

    void Start()
    {
        startingYPosition = transform.position.y;
        destroyMarker = startingYPosition - (CAMERA_FRAME_HEIGHT * 2f);
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < destroyMarker)
        {
            Destroy(gameObject);
        }
	}
}
