using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuleManager : SingletonMonoBehaviour<RuleManager>
{


    public enum infectationsTypes { P1 = 1, P2, P3, P4 };
    public enum collisionType { MeInfectOther, Nothing , OtherInfectMe };
    public bool activeRules = true;
    static RuleManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = DestroyOptions.DestroyBehaviour;
        Persist = true;
    }


    [SerializeField] //                            0                        1                  2                     3
    int[] infectionRole = { (int)infectationsTypes.P1, (int)infectationsTypes.P2, (int)infectationsTypes.P3, (int)infectationsTypes.P4 };

    private void reshuffle(int[] infectionRole)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < infectionRole.Length; t++)
        {
            int tmp = infectionRole[t];
            int r = Random.Range(t, infectionRole.Length);
            infectionRole[t] = infectionRole[r];
            infectionRole[r] = tmp;
        }
    }

    public void NextRules() {
        reshuffle(infectionRole);
    }

  


    public collisionType CompareInfectation(int infect1, int infect2)
    {

        int indexInfect1 = 0;

        if (infect1 == 0 && infect2 == 0) {
            return collisionType.Nothing;
        }

        if (infect1 == 0) {
            return collisionType.OtherInfectMe;
        }
        else if (infect2 == 0) {
            return collisionType.MeInfectOther;
        }


        for (int i = 1; i < infectionRole.Length; i++) {
            if (infectionRole[i] ==  infect1) {
                indexInfect1 = i;
                break;
            }
        }

        int nextCheck = (indexInfect1 + 1) % 4;
        int previousCheck = (indexInfect1 - 1) < 0 ? indexInfect1 + 3 : indexInfect1 - 1;


        //Check if I can infect the other
        if (infectionRole[nextCheck] ==  infect2){
            return collisionType.MeInfectOther;
        }

        if (infectionRole[previousCheck] == infect2)
        {
            return collisionType.OtherInfectMe;
        }

        return collisionType.Nothing;
    }
}