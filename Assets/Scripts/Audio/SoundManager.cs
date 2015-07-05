using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

public class SoundManager : MonoBehaviour {

    public float playerSoundVolume;
    public float enemySoundVolume;
    public float weaponSoundVolume;
    public float pickupSoundVolume;

    private string FILE_PATH;

    private static SoundManager _instance;

    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
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
            FILE_PATH = Application.persistentDataPath + "/sound.dat";
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
        new BinaryFormatter().Serialize(file, new SoundManagerInstance(playerSoundVolume, enemySoundVolume, weaponSoundVolume, pickupSoundVolume));
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
            SoundManagerInstance settings = (SoundManagerInstance)bf.Deserialize(file);
            playerSoundVolume = settings.playerSoundVolume;
            enemySoundVolume = settings.enemySoundVolume;
            weaponSoundVolume = settings.weaponSoundVolume;
            pickupSoundVolume = settings.pickupSoundVolume;
            file.Close();
        }
        else
        {
            SetDefaultValues();
        }
    }

    private void SetDefaultValues()
    {
        playerSoundVolume = 1;
        enemySoundVolume = 1;
        weaponSoundVolume = 1;
        pickupSoundVolume = 1;
    }


}


[Serializable]
class SoundManagerInstance
{

    public float playerSoundVolume;
    public float enemySoundVolume;
    public float weaponSoundVolume;
    public float pickupSoundVolume;

    public SoundManagerInstance(float playerSoundVolume, float enemySoundVolume, float weaponSoundVolume, float pickupSoundVolume)
    {
        this.playerSoundVolume = playerSoundVolume;
        this.enemySoundVolume = enemySoundVolume;
        this.weaponSoundVolume = weaponSoundVolume;
        this.pickupSoundVolume = pickupSoundVolume;
    }

}
