using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : SingletonMonoBehaviour<GameplayManager> {

    public float timeToChangeRule;
    public List<Player> players = new List<Player>();
    public List<Infectable> infectables = new List<Infectable>();

    [Header("UI")]
    [SerializeField] Text P1Score;
    [SerializeField] Text P2Score;
    [SerializeField] Text P3Score;
    [SerializeField] Text P4Score;
    [SerializeField] Image[] Rules;

    static GameplayManager()
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
        InvokeRepeating("ChangeRules", 0, 15);
    }

    public void Update()
    {
    }

    public void ChangeInfectableOwner(Player playerOrigin, Player playerDest) {
        for (int i = 0; i < infectables.Count; i++) {
            if(infectables[i].infectedBy != null && infectables[i].infectedBy.GetComponent<Player>().playerIndex == playerOrigin.playerIndex)
            {
                infectables[i].Infect(playerDest);
            }
        } 
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
        for (int i = 0; i < 4; i++) {
            Rules[(i + 1) % 4].color = GameManager.Instance.GetPlayerColor(RuleManager.Instance.GetRule(i));
        }
    }
}
