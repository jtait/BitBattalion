using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MusicManager : MonoBehaviour {

    private const float FADE_INCREMENT = 0.035f;
    private const float FADE_RESOLUTION = 0.03f;

    private Dictionary<string, AudioClip> songList = new Dictionary<string, AudioClip>();

    private AudioClip storylevel1;
    private AudioClip storylevel1_boss;
    private AudioClip storylevel2;
    private AudioClip storylevel2_boss;
    private AudioClip storylevel3;
    private AudioClip storylevel3_boss;
    private AudioClip endlessMode1;
    private AudioClip endlessMode2;
    private AudioClip menu;

    private AudioSource aSource;
    private bool fadeIn = false;
    private string songName;

    private static MusicManager _instance;

    public static MusicManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MusicManager>();
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

            aSource = GetComponent<AudioSource>();

            /* load songs */
            storylevel1 = Resources.Load<AudioClip>("Music/While_Atlantis_Sleeps");
            storylevel1_boss = Resources.Load<AudioClip>("Music/The_Final_Threat");
            storylevel2 = Resources.Load<AudioClip>("Music/OHC_Mechanized_Whalesong");
            storylevel2_boss = Resources.Load<AudioClip>("Music/Chaos_Jungle");
            storylevel3 = Resources.Load<AudioClip>("Music/Fading_World_v1_1");
            storylevel3_boss = Resources.Load<AudioClip>("Music/OHC_Changeling_Rumble");
            menu = Resources.Load<AudioClip>("Music/Patashu_in_the_Stars");
            endlessMode1 = Resources.Load<AudioClip>("Music/Starship_Orion");
            endlessMode2 = Resources.Load<AudioClip>("Music/Dark_Nebula");

            /* add all songs to dictionary */
            songList.Add("Story_Level_01", storylevel1);
            songList.Add("Story_Level_02", storylevel2);
            songList.Add("Story_Level_03", storylevel3);
            songList.Add("Story_Level_01_Boss", storylevel1_boss);
            songList.Add("Story_Level_02_Boss", storylevel2_boss);
            songList.Add("Story_Level_03_Boss", storylevel3_boss);
            songList.Add("menu", menu);
            songList.Add("Endless_Mode_01", endlessMode1);
            songList.Add("Endless_Mode_02", endlessMode2);

        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }


    /* switch the song */
    public void NewSong(string song, bool fade)
    {
        songName = song;

        if (fade)
        {            
            StopCoroutine(ChangeSong());
            fadeIn = false;
            StartCoroutine(ChangeSong());
        }

        else
        {
            aSource.clip = songList[songName];
            aSource.Play();
        }
        
    }

    /* handle crossfading */
    IEnumerator ChangeSong()
    {
        while(aSource.volume > 0 && !fadeIn)
        {
            aSource.volume -= FADE_INCREMENT;
            yield return new WaitForSeconds(FADE_RESOLUTION);
        }

        aSource.clip = songList[songName];
        aSource.Play();
        fadeIn = true;

        while(aSource.volume < 1 && fadeIn)
        {
            aSource.volume += FADE_INCREMENT;
            yield return new WaitForSeconds(FADE_RESOLUTION);
        }
        
        fadeIn = false;
    }

}
