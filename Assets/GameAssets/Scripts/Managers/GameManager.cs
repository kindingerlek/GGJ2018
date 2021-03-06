﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    [Header("Setup")]
    [SerializeField] Color P1Color;
    [SerializeField] Color P2Color;
    [SerializeField] Color P3Color;
    [SerializeField] Color P4Color;

    [SerializeField] GameObject[] PlayerSprites;

    readonly HashSet<int> playersEnabled = new HashSet<int>();

    static GameManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = DestroyOptions.DestroyGameObject;
        Persist = true;
    }

    public void SetPlayerPrefab(int i, GameObject prefab)
    {
        PlayerSprites[i - 1] = prefab;
    }

    public GameObject GetPlayerSpritePrefab(int i)
    {
        if (i <= 0 || i > PlayerSprites.Length) {
            return null;
        }
        var prefab = PlayerSprites[i - 1];
        return prefab;
    }

    public bool GetPlayerEnabled(int player) {
        if (playersEnabled.Count <= 0)
			return true;
		var res = playersEnabled.Contains(player);
        return res;
	}

    public void SetPlayerEnabled(int i, bool value)
    {
        if (value)
            playersEnabled.Add(i);
        else
            playersEnabled.Remove(i);
    }

    public Color GetPlayerColor(int i)
    {
        Color[] colors = { Color.white, P1Color, P2Color, P3Color, P4Color };

        return i >= colors.Length ? colors[0] : colors[i];
    }
}
