﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    public float timeToChangeRule;
    public List<Player> players = new List<Player>();
    public List<Infectable> infectables = new List<Infectable>();


    [Header("UI")]
    public Text P1Score;
    public Text P2Score;
    public Text P3Score;
    public Text P4Score;

    static GameManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = DestroyOptions.DestroyBehaviour;
        Persist = true;
    }
    

    public int AddPlayer(Player player)
    {
        int count = players.Count;

        if (count >= 4)
            return -1;

        players.Add(player);

        return count;
    }

    protected override void Awake()
    {
        base.Awake();
        UpdatePoints();
        InvokeRepeating("ChangeRules", 5, 10);
    }

    public void Update()
    {
    }

    public void UpdatePoints()
    {
        P1Score.text = "P1: " + players[0].points;
        P2Score.text = "P2: " + players[1].points;
        P3Score.text = "P3: " + players[2].points;
        P4Score.text = "P4: " + players[3].points;
    }

    void ChangeRules()
    {
        RuleManager.Instance.NextRules();
    }
}
