using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

public class HighScoreTable : MonoBehaviour {

    private string FILE_PATH;
    private const int NUMBER_OF_ENTRIES = 10;
    private Score[] campaignScores = new Score[NUMBER_OF_ENTRIES];
    private Score[] endlessScores = new Score[NUMBER_OF_ENTRIES];

    public enum TableType { CAMPAIGN, ENDLESS };

    private static HighScoreTable _instance;

    public static HighScoreTable instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<HighScoreTable>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            FILE_PATH = Application.persistentDataPath + "/highScoreTable.dat";
            Load();
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    public Score[] GetTable(TableType type)
    {
        switch (type)
        {
            case TableType.CAMPAIGN:
                return campaignScores;
            case TableType.ENDLESS:
                return endlessScores;
            default:
                return null;
        }
    }

    private bool ScoreShouldBeAdded(long scoreValue, TableType type)
    {
        return scoreValue >= GetTable(type)[NUMBER_OF_ENTRIES - 1].scoreValue;
    }

    /* add score to table, return true if added, false if not */
    public bool AddScore(String name, long scoreValue, TableType type)
    {
        if (ScoreShouldBeAdded(scoreValue, type))
        {
            int i = 0;
            Score[] tableToAdd = GetTable(type);

            // find the appropropriate spot to add the score
            while (i < NUMBER_OF_ENTRIES)
            {
                if (tableToAdd[i].scoreValue <= scoreValue)
                {
                    ShuffleDown(i, type);
                    break;
                }
                else
                {
                    i++;
                }
            }

            if (i < NUMBER_OF_ENTRIES)
            {
                tableToAdd[i] = new Score(name, scoreValue);
                return true;
            }
        }

        return false;
    }

    /* shuffle lower scores down to make room for a new score */
    private void ShuffleDown(int index, TableType type){

        Score[] tableToShuffle = GetTable(type);

        for (int i = NUMBER_OF_ENTRIES - 1; i > index; i--)
        {
            tableToShuffle[i] = tableToShuffle[i - 1];
        }
    }

    
    public void Save()
    {
#if UNITY_STANDALONE
        SaveLocal();
#elif UNITY_EDITOR
        SaveLocal();
#endif
    }

    private void SaveLocal()
    {
        FileStream file = File.Open(FILE_PATH, FileMode.Create);
        new BinaryFormatter().Serialize(file, new HighScoreTableInstance(campaignScores, endlessScores));
        file.Close();
    }

    public void Load()
    {
#if UNITY_STANDALONE
        LoadLocal();
#elif UNITY_EDITOR
        LoadLocal();
#else
        CreateEmptyTables();
#endif
    }

    private void LoadLocal()
    {
        if (File.Exists(FILE_PATH))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(FILE_PATH, FileMode.Open);
            HighScoreTableInstance table = (HighScoreTableInstance)bf.Deserialize(file);
            campaignScores = table.campaignScores;
            endlessScores = table.endlessScores;
            file.Close();
        }
        else
        {
            CreateEmptyTables();
        }
    }

    private void CreateEmptyTables()
    {
        for (int i = 0; i < NUMBER_OF_ENTRIES; i++)
        {
            campaignScores[i] = new Score("name", 0);
            endlessScores[i] = new Score("name", 0);
        }
    }

}

[Serializable]
class HighScoreTableInstance
{

    public Score[] campaignScores;
    public Score[] endlessScores;

    public HighScoreTableInstance(Score[] campaign, Score[] endless)
    {
        campaignScores = campaign;
        endlessScores = endless;
    }

}

[Serializable]
public class Score
{
    public string playerName;
    public long scoreValue;

    public Score(string playerName, long scoreValue)
    {
        this.playerName = playerName;
        this.scoreValue = scoreValue;
    }
}
