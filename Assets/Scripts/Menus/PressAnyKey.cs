using UnityEngine;
using System.Collections;

public class PressAnyKey : MonoBehaviour {

    public string sceneToLoad;

    void OnMouseDown()
    {
        Application.LoadLevel(sceneToLoad);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Application.LoadLevel(sceneToLoad);
        }
    }
}
