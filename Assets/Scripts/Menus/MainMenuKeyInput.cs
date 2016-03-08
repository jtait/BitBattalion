using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/* handles keyboard input on main menu */
public class MainMenuKeyInput : MonoBehaviour {

    private const string CAMPAIGN = "Story_Level_01";
    private const string ENDLESS = "Endless_Mode";
    private const string HIGH_SCORE = "Menu_Highscores";
    private const string OPTIONS = "Menu_Options";
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(CAMPAIGN);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(ENDLESS);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene(HIGH_SCORE);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(OPTIONS);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
	}
}
