using UnityEngine;
using System.Collections;

public class LoadSceneButton : MonoBehaviour {

    public string sceneToLoad;

    void OnMouseDown()
    {
        Application.LoadLevel(sceneToLoad);
    }
}
