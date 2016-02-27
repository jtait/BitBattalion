using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour {

    public string sceneToLoad;

    void OnMouseDown()
    {
        this.LoadScene(sceneToLoad);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
