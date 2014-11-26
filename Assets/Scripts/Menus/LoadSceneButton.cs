using UnityEngine;
using System.Collections;

public class LoadSceneButton : MonoBehaviour {

    public int sceneToLoad;

    void OnMouseDown()
    {
        Application.LoadLevel(sceneToLoad);
    }
}
