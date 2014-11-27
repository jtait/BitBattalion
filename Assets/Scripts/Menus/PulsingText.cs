using UnityEngine;
using System.Collections;

public class PulsingText : MonoBehaviour {

    private GUIText startText;
    private bool fadeIn = false;

	void Start () {
        startText = gameObject.GetComponent<GUIText>();
	}

    void Update()
    {
        Pulse();
    }

    void FadeOut()
    {
        Color temp = startText.material.color;
        temp.a = temp.a - (float)0.5 * Time.deltaTime;
        startText.material.color = temp;
    }

    void FadeIn()
    {
        Color temp = startText.material.color;
        temp.a = temp.a + (float)0.5 * Time.deltaTime;
        startText.material.color = temp;
    }

    void Pulse()
    {
        if(guiText.material.color.a > 0 && !fadeIn)
        {
            FadeOut();
        }
        else
            fadeIn = true;
        if (guiText.material.color.a < 1 && fadeIn)
        {
            FadeIn();
        }
        else
            fadeIn = false;
    }

}
