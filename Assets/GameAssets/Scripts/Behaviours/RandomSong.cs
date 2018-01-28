using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSong : MonoBehaviour {

	public List<AudioClip> musics;

	// Use this for initialization
	void Start () {
		
	}
	
	public AudioClip GetRandomSong() {
		return musics.PickRandom();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
