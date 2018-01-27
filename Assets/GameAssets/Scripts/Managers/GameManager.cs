using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    private RuleManager ruleManager;
    static GameManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = true;
        Persist = true;
    }

    public RuleManager GetRuleManager() {
        return this.ruleManager;
    }


    public List<Player> players = new List<Player>();

    public int CompareInfectation(Infectable inf1, Infectable inf2) {
        return ruleManager.CompareInfectation(inf1,inf2);
    }

    public int AddPlayer(Player player)
    {
        int count = players.Count;

        if (count >= 4)
            return -1;

        players.Add(player);

        return count;
    }


}
