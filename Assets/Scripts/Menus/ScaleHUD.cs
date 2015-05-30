using UnityEngine;
using System.Collections;

public class ScaleHUD : MonoBehaviour {

    public int SCALE_FACTOR;

    private int screenWidth;

    void Awake()
    {
        screenWidth = Screen.width;
        GetComponent<GUIText>().fontSize = screenWidth / SCALE_FACTOR;
    }

}
