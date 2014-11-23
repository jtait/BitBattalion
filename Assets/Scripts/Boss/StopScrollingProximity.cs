using UnityEngine;
using System.Collections;

public class StopScrollingProximity : MonoBehaviour {

    private Transform playerTransform; // the transform of the player
    private float yPosition; // the y position of this object
    private const float yOffset = -2; // the distance to the player when scrolling stops



	// Use this for initialization
	void Start () {
        playerTransform = GameObject.Find("Player").transform;
        yPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerTransform.position.y + yOffset > yPosition)
        {
            playerTransform.GetComponent<ConstantScroll>().isActive = false;
        }
	}
}
