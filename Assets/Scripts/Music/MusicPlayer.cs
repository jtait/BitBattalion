using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public string songTitle;
    public bool fade;

	void Start () {
        MusicManager.instance.NewSong(songTitle, fade);
	}
}
