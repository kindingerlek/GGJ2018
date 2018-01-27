
﻿using System.Collections;
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

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
