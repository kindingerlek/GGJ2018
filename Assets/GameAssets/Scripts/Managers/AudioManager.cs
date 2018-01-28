
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
	public List<AudioClip> musics;


	public void SetMusic(AudioClip clip)
	{
		if (playlist) {
			PlayNextSong();
		} else {
			if (clip == null) {
				audioSource.Stop();
				audioSource.clip = null;
			}
		else if (audioSource.clip != clip) {
			audioSource.clip = clip;
			audioSource.Play();
			}
		}
		
	}

	public bool playlist = false;

	void PlayNextSong () {
		audioSource.clip = musics.PickRandom();
		audioSource.Play();
		Invoke("PlayNextSong", audioSource.clip.length); //vai invocar a função no tempo colocado depois da vírgula.
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public AudioClip rule;
	public void ChangedRule () {
		audioSource.PlayOneShot(rule);
	} 
}
