using UnityEngine;
using System.Collections;

public class GameParameters : MonoBehaviour {

    public int difficulty;
    private long playerScore;
    private const long maxScore = 999999999999L;

    void Start()
    {
        difficulty = 1;
        playerScore = 0;
    }

    /* update the player's score - return true if score has changed */
    public bool UpdateScore(long points)
    {
        long oldScore = playerScore;
        playerScore += points;

        return (playerScore != oldScore);

    }

    /* return the player's score */
    public long getPlayerScore()
    {
        return playerScore;
    }

}
