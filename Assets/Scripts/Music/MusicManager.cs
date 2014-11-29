using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MusicManager : MonoBehaviour {

    private const float FADE_CONSTANT = 0.9f;

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
    private bool change;
    private string songName;

	void Awake ()
    {
        DontDestroyOnLoad(gameObject);

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

        /* default song */
        aSource.clip = menu;
        aSource.Play();
	}

    /* switch the song */
    public void NewSong(string song)
    {
        songName = song;
        change = true;
    }

    /* handle crossfading */
    void Update(){
        if (change)
        {
            if (aSource.volume > 0 && !fadeIn)
            {
                FadeOut();
            }
            else if(!fadeIn)
            {
                aSource.clip = songList[songName];
                aSource.Play();
                fadeIn = true;
            }

            if (aSource.volume < 1 && fadeIn)
            {
                FadeIn();
            }
            else if(fadeIn)
            {
                fadeIn = false;
                change = false;
            }
        }
    }

    void FadeOut()
    {
        aSource.volume -= FADE_CONSTANT * Time.deltaTime;
    }

    void FadeIn()
    {
        aSource.volume += FADE_CONSTANT * Time.deltaTime;
    }



}
