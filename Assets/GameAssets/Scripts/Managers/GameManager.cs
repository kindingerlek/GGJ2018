using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    public float timeToChangeRule;
    public List<Player> players = new List<Player>();


    [Header("UI")]
    public Text P1Score;
    public Text P2Score;
    public Text P3Score;
    public Text P4Score;

    static GameManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = true;
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
        InvokeRepeating("CountPoints", 5, 10);
    }

    public void Update()
    {
    }

    void CountPoints()
    {
        int[] points =  new int[5];

        Infectable[] infectables = GameObject.FindObjectsOfType<Infectable>();

        foreach(var infect in infectables)
        {
            if (infect.infectedBy == null)
                continue;

            int i = infect.infectedBy.GetComponent<Player>().playerIndex;

            points[i]++;
        }


        P1Score.text = "P1: " + points[1];
        P2Score.text = "P2: " + points[2];
        P3Score.text = "P3: " + points[3];
        P4Score.text = "P4: " + points[4];
    }


}
