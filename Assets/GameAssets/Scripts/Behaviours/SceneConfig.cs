using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfig : MonoBehaviour {
	[SerializeField] AudioClip bgm;

	// Use this for initialization
	void Start () {
		AudioManager.Instance.SetMusic(bgm);
	}
}