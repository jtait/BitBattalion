using UnityEngine;
using System.Collections;

public class BossProximity : MonoBehaviour {

    private Transform playerTransform; // the transform of the player
    private float yPosition; // the y position of this object
    private const float yOffset = -2; // the distance to the player when scrolling stops

    private MusicPlayer musicPlayer;
    private string regularSong;
    private string bossSong;
    private MusicManager manager;
    private bool changedSong = false;

	// Use this for initialization
	void Start () {
        try
        {
            manager = MusicManager.instance;
            musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
            regularSong = musicPlayer.songTitle;
            bossSong = regularSong + "_Boss";
        }
        catch (System.NullReferenceException)
        {
            manager = null;
        }
        
        playerTransform = GameObject.Find("Player").transform;
        yPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerTransform.position.y + yOffset > yPosition)
        {
            playerTransform.GetComponent<ConstantScroll>().isActive = false;
            if (!changedSong)
            {
                if(manager != null) manager.NewSong(bossSong, true);
                changedSong = true;
            }
        }
        else
        {
            playerTransform.GetComponent<ConstantScroll>().isActive = true;
        }
	}
}
