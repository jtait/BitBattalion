using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsMenuWorker : MonoBehaviour {

    void Start()
    {
        GameObject.Find("SoundFXSlider").GetComponent<Slider>().value = SettingsManager.instance.soundEffectVolume;
        GameObject.Find("MusicSlider").GetComponent<Slider>().value = SettingsManager.instance.musicVolume;
    }

    public void setMusicVolume(float volume)
    {
        SettingsManager.instance.musicVolume = volume;
        saveSettings();
    }

    public void setSFXVolume(float volume)
    {
        SettingsManager.instance.soundEffectVolume = volume;
        saveSettings();
    }

    private void saveSettings()
    {
        SettingsManager.instance.Save();
    }
}
