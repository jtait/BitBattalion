using UnityEngine;
using System.Collections;

public class GameOverScoreDisplay : MonoBehaviour {

    private GUIText scoreText;
    private GameParameters gParams;

	void Awake () {

        scoreText = GameObject.Find("ScoreDisplayText").GetComponent<GUIText>();

        try
        {
            gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
            scoreText.text = "Your final score was " + gParams.GetPlayerScore().ToString();
        }
        catch (System.NullReferenceException)
        {
            scoreText.text = "error";
        }
        
	}

}
