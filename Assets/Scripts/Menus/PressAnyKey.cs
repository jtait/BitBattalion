using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour {

    public string sceneToLoad;

    void OnMouseDown()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
