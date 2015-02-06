using UnityEngine;
using System.Collections;

public class DisplayHighScores : MonoBehaviour {

    private GUIText campaign_mode_text;
    private GUIText endless_mode_text;

	void Start () {
        GUIText[] guiStrings = gameObject.GetComponentsInChildren<GUIText>();
        campaign_mode_text = guiStrings[0];
        endless_mode_text = guiStrings[1];
        Score[] campaignScores = HighScoreTable.instance.GetTable(HighScoreTable.TableType.CAMPAIGN);
        Score[] endlessScores = HighScoreTable.instance.GetTable(HighScoreTable.TableType.ENDLESS);

        campaign_mode_text.text = BuildString(campaignScores);
        endless_mode_text.text = BuildString(endlessScores);

	}

    private string BuildString(Score[] scores)
    {
        string theString = "";

        int length = scores.Length;
        for (int i = 0; i < length; i++)
        {
            theString += scores[i].playerName + " " + scores[i].scoreValue + "\n";
        }

        return theString;
    }

}
