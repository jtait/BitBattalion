using UnityEngine;
using System.Collections;

public class ConstantScroll : MonoBehaviour {

    public float scrollSpeed = 0.05f; // how fast the object moves in the y direction

    public bool isActive = true; // if the scroll is active or not

    private float xPos;
    private float zPos;

    private float speedMultiplier;

    void Awake()
    {
        speedMultiplier = (float) GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>().speedMultiplier; // set the difficulty parameter of the enemy to the difficulty of the game when the enemy is created
        scrollSpeed = scrollSpeed * speedMultiplier;
        print(speedMultiplier);
    }

	// Use this for initialization
	void Start () {
        xPos = transform.position.x;
        zPos = transform.position.z;	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive) transform.position = new Vector3(xPos, transform.position.y + scrollSpeed, zPos);
        else transform.position = new Vector3(xPos, transform.position.y, zPos);
	}

}
