using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {

    void OnMouseDown()
    {
        Application.Quit();
    }
}
