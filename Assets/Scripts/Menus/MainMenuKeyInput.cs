using UnityEngine;
using System.Collections;

/* handles keyboard input on main menu */
public class MainMenuKeyInput : MonoBehaviour {

    private const string CAMPAIGN = "Story_Level_01";
    private const string ENDLESS = "Endless_Mode";
    private const string HIGH_SCORE = "Menu_Main";
    private const string OPTIONS = "Menu_Options";
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.C))
        {
            Application.LoadLevel(CAMPAIGN);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Application.LoadLevel(ENDLESS);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Application.LoadLevel(HIGH_SCORE);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Application.LoadLevel(OPTIONS);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
	}
}
