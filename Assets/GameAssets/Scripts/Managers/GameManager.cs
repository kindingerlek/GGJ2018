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

    static GameManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = DestroyOptions.DestroyBehaviour;
        Persist = true;
    }

    public Color GetPlayerColor(int i)
    {
        Color[] colors = { Color.white, P1Color, P2Color, P3Color, P4Color };

        return i >= colors.Length ? colors[0] : colors[i];
    }
}
