using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    public float timeToChangeRule;
    public List<Player> players = new List<Player>();
    public List<Infectable> infectables = new List<Infectable>();

    [Header("Setup")]
    [SerializeField] Color P1Color;
    [SerializeField] Color P2Color;
    [SerializeField] Color P3Color;
    [SerializeField] Color P4Color;
    
    [Header("UI")]
    [SerializeField] Text P1Score;
    [SerializeField] Text P2Score;
    [SerializeField] Text P3Score;
    [SerializeField] Text P4Score;

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

    public Color getPlayerColor(int i)
    {
        Color[] colors = { Color.white, P1Color, P2Color, P3Color, P4Color };

        return i >= colors.Length ? colors[0] : colors[i];
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
