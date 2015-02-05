using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

public class HighScoreTable : MonoBehaviour {

    private string FILE_PATH = Application.persistentDataPath + "/highScoreTable.dat";
    private const int NUMBER_OF_ENTRIES = 10;
    private Score[] scores = new Score[NUMBER_OF_ENTRIES];

    private static HighScoreTable _instance;

    public static HighScoreTable instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<HighScoreTable>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    public Score[] GetTable()
    {
        return this.scores;
    }

    public bool CheckIfScoreShouldBeAdded(long scoreValue)
    {
        return scoreValue >= scores[NUMBER_OF_ENTRIES - 1].scoreValue;
    }

    /* add score to table, return true if added, false if not */
    public bool AddScore(String name, long scoreValue)
    {
        int i = 0;

        // find the appropropriate spot to add the score
        while (i < NUMBER_OF_ENTRIES)
        {
            if (scores[i].scoreValue <= scoreValue)
            {
                ShuffleDown(i);
            }
            else
            {
                i++;
            }
        }

        if (i < NUMBER_OF_ENTRIES)
        {
            scores[i] = new Score(name, scoreValue);
            return true;
        }

        return false;
    }

    /* shuffle lower scores down to make room for a new score */
    private void ShuffleDown(int index){
        for (int i = NUMBER_OF_ENTRIES - 1; i > index; i--)
        {
            scores[i] = scores[i - 1];
        }
    }

    [Conditional("UNITY_STANDALONE")]
    public void Save()
    {
        FileStream file = File.Open(FILE_PATH, FileMode.Create);
        new BinaryFormatter().Serialize(file, new HighScoreTableInstance(scores));
        file.Close();
    }

    [Conditional("UNITY_STANDALONE")]
    public void Load()
    {
        if (File.Exists(FILE_PATH))
        {
            FileStream file = File.Open(FILE_PATH, FileMode.Open);
            scores = ((HighScoreTableInstance) new BinaryFormatter().Deserialize(file)).score;
        }
    }

}

[Serializable]
class HighScoreTableInstance
{

    public Score[] score;

    public HighScoreTableInstance(Score[] scores)
    {
        score = scores;
    }

}

[Serializable]
class Score
{
    public string playerName;
    public long scoreValue;

    public Score(string playerName, long scoreValue)
    {
        this.playerName = playerName;
        this.scoreValue = scoreValue;
    }
}
