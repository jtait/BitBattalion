using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public string songTitle;
    MusicManager manager;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

	void Start () {
        manager.NewSong(songTitle);
	}
}
