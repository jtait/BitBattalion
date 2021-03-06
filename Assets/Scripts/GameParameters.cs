﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameParameters : MonoBehaviour {

    public int playerLives;
    public int difficulty;
    private long playerScore;
    public Vector3 lastCheckpoint;
    public float speedMultiplier = 1; // global speed multiplier for enemy movement

    public int endlessModeTurrets = 0;
    public bool endlessMode = false;

    /* parameters for HUD */
    private GUIText livesString;
    private GUIText scoreString;

    /* table of all enemies on screen */
    private ArrayList enemyList;

    /* pausing */
    public bool paused = false;

    private static GameParameters _instance;

    public static GameParameters instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameParameters>();
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

            lastCheckpoint = Vector3.zero;

            difficulty = 1;
            playerScore = 0;
            playerLives = 5;
            Reset();

        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    void Reset()
    {
        enemyList = new ArrayList(); // reset the enemy list

        GameObject hud = GameObject.Find("HUD");
        if (hud != null)
        {
            GUIText[] guiStrings = hud.GetComponentsInChildren<GUIText>();
            livesString = guiStrings[0];
            scoreString = guiStrings[1];
        }
        UpdateScore(0);
        if (livesString != null)
        {
            SetLivesText();
        }
    }

    void OnLevelWasLoaded()
    {
        Reset();
    }

    /* update the player's score - return true if score has changed */
    public bool UpdateScore(long points)
    {
        long oldScore = playerScore;
        playerScore += points;
        if(scoreString != null)
            scoreString.text = playerScore.ToString(); // update the display string
        return (playerScore != oldScore);
    }

    /* return the player's score */
    public long GetPlayerScore()
    {
        return playerScore;
    }

    /* update the life display */
    public void SetLivesText()
    {
        livesString.text = playerLives.ToString();
    }

    /* add enemy to list of enemies on screen */
    public void AddEnemyToList(GameObject enemy)
    {
        enemyList.Add(enemy);
    }

    /* remove enemy from list of enemies on screen */
    public void RemoveEnemyFromList(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }

    /* destroy all the enemies in the list */
    public void DestroyAllEnemiesInList()
    {
        /* loop through list and destroy all enemies in the list */
        foreach (GameObject enemy in enemyList)
        {
            if(enemy != null)
                Destroy(enemy);
        }
        enemyList = new ArrayList(); // reset the list
    }

    /* destroy all the enemies in the list */
    public void CallDeathOnAllEnemiesInList()
    {
        /* copy list */
        ArrayList listToKill = new ArrayList();
        listToKill.AddRange(enemyList);
        /* loop through list and destroy all enemies in the list */
        foreach (GameObject enemy in listToKill)
        {
            if (enemy != null)
            {
                enemyList.Remove(enemy);
                enemy.SendMessage("Death", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    /* pause the game */
    public void PauseGame(){
        Object[] objects = FindObjectsOfType (typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        }
        Time.timeScale = 0;
        paused = true;
    }

    /* resume the game */
    public void ResumeGame(){
        Time.timeScale = 1;
        Object[] objects = FindObjectsOfType (typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
        }
        paused = false;
    }

    /* wait for a specific time, then load the next level */
    public static IEnumerator WaitForLevelLoad(float waitTime, string level)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(level);
    }

}
