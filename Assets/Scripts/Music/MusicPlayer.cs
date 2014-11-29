using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public string songTitle;

	void Start () {
        MusicManager manager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
        manager.NewSong(songTitle);
	}
}
