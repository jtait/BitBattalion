using UnityEngine;
using System.Collections;

public class ConstantScroll : MonoBehaviour {

    public float scrollSpeed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(0, transform.position.y + scrollSpeed, 0);
	}

}
