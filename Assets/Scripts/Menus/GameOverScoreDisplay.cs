using UnityEngine;
using System.Collections;

public class GameOverScoreDisplay : MonoBehaviour {

    private GUIText scoreText;
    private GameParameters gParams;

	void Awake () {

        scoreText = GameObject.Find("ScoreDisplayText").GetComponent<GUIText>();
        gParams = GameParameters.instance;
               
	}

    void Start()
    {
        scoreText.text = "Your final score was " + gParams.GetPlayerScore().ToString();
        if (gParams.endlessMode)
        {
            HighScoreTable.instance.AddScore("player", gParams.GetPlayerScore(), HighScoreTable.TableType.ENDLESS);
        }
        else
        {
            HighScoreTable.instance.AddScore("player", gParams.GetPlayerScore(), HighScoreTable.TableType.CAMPAIGN);
        }
        HighScoreTable.instance.Save();
    }

}
