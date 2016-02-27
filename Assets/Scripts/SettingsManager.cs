using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

public class SettingsManager : MonoBehaviour {

    public float soundEffectVolume { get; set; }
    public float musicVolume { get; set; }
    public Dictionary<string, float> volumeDict;

    private string fileName = "/settings.dat";
    private string FILE_PATH;
    
    private static SettingsManager _instance;

    public static SettingsManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SettingsManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            /* load settings from file */
            FILE_PATH = Application.persistentDataPath + fileName;
            Load();
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
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
        new BinaryFormatter().Serialize(file, new SettingsManagerInstance(soundEffectVolume, musicVolume, volumeDict));
        file.Close();
    }

    public void Load()
    {
#if UNITY_STANDALONE
        LoadLocal();
#elif UNITY_EDITOR
        LoadLocal();
#else
        SetDefaultValues();
#endif
    }

    private void LoadLocal()
    {
        if (File.Exists(FILE_PATH))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(FILE_PATH, FileMode.Open);
            SettingsManagerInstance settings = (SettingsManagerInstance)bf.Deserialize(file);
            soundEffectVolume = settings.soundEffectVolume;
            musicVolume = settings.musicVolume;
            volumeDict = settings.volumeDict;
            file.Close();
        }
        else
        {
            SetDefaultValues();
        }
    }

    private void SetDefaultValues()
    {
        soundEffectVolume = 1;
        musicVolume = 1;
        volumeDict = new Dictionary<string, float>();
        volumeDict.Add("player", 1);
        volumeDict.Add("enemy", 1);
        volumeDict.Add("weapon", 1);
        volumeDict.Add("pickup", 1);
    }


}


[Serializable]
class SettingsManagerInstance
{

    public float soundEffectVolume;
    public float musicVolume;
    public Dictionary<string, float> volumeDict;

    public SettingsManagerInstance(float soundEffectVolume, float musicVolume, Dictionary<string, float> volumeDict)
    {
        this.soundEffectVolume = soundEffectVolume;
        this.musicVolume = musicVolume;
        this.volumeDict = volumeDict;
    }

}
