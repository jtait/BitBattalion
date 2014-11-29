using UnityEngine;
using System.Collections;

public class ScaleHUD : MonoBehaviour {

    public int SCALE_FACTOR;

    private int screenWidth;

    void Awake()
    {
        screenWidth = Screen.width;
    }

    void OnGUI()
    {
        guiText.fontSize = screenWidth / SCALE_FACTOR;
    }
}
