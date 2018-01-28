
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	static AudioManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = DestroyOptions.DestroyGameObject;
        Persist = true;
    }

	[SerializeField] AudioSource audioSource;

	public void SetMusic(AudioClip clip)
	{
		if (clip == null) {
			audioSource.Stop();
			audioSource.clip = null;
		}
		else if (audioSource.clip != clip) {
			audioSource.clip = clip;
			audioSource.Play();
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
