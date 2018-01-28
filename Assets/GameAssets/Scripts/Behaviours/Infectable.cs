using System;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class Infectable : MonoBehaviour {
    
    public Player infectedBy = null;

    void Start()
    {
        GameplayManager.Instance.infectables.Add(this);
    }

    void OnDestroy()
    {
        if (GameplayManager.Instance) {
            GameplayManager.Instance.infectables.Remove(this);
        }
    }

    public void ValidateInfectioNpc(Infectable infectable) {

        if (infectable == null)
            return;

        var i1 = infectedBy == null ? 0 : infectedBy.playerIndex;
        var i2 = infectable.infectedBy == null ? 0 : infectable.infectedBy.playerIndex;

        RuleManager.collisionType infectRes = RuleManager.Instance.CompareInfectation(i1, i2);
        if (infectRes == RuleManager.collisionType.MeInfectOther)
        {
            Debug.Log(this.name + ": I infect " + infectable.name);
            infectable.Infect(infectedBy);
        }
        else if (infectRes == RuleManager.collisionType.OtherInfectMe)
        {
            Debug.Log(this.name + ": I was infected by " + infectable.name);
            Infect(infectable.infectedBy);
        }

    }

    public void ValidateInfectioPlayer(Player player)
    {

        if (player == null)
            return;

        var i1 = infectedBy == null ? 0 : infectedBy.playerIndex;
        var i2 = player.playerIndex == null ? 0 : player.playerIndex;

        RuleManager.collisionType infectRes = RuleManager.Instance.CompareInfectation(i1, i2);
        if (infectRes == RuleManager.collisionType.OtherInfectMe)
        {
            Debug.Log(this.name + ": I was infected by " + player.name);
            Infect(player);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        Infectable infectable = collision.gameObject.GetComponent<Infectable>();

        ValidateInfectioNpc(infectable);

    }


    public void Infect(Player other)
    {
        if (other == infectedBy)
            return;

        if (infectedBy != null) {
            infectedBy.points -= 1;
        }

        infectedBy = other;

        if (infectedBy) {
            infectedBy.points += 1;
        }

        GetComponent<NPC2>().npcIndicator.color = GameManager.Instance.GetPlayerColor(other.playerIndex);

        GameplayManager.Instance.UpdatePoints();
    }
}
