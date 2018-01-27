using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    static GameManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = true;
        Persist = true;
    }


    public List<Player> players = new List<Player>();

    public int AddPlayer(Player player)
    {
        int count = players.Count;

        if (count >= 4)
            return -1;

        players.Add(player);

        return count;
    }


}
