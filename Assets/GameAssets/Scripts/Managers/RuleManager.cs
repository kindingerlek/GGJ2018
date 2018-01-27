using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuleManager : SingletonMonoBehaviour<RuleManager>
{


    public enum infectationsTypes { P1 = 1, P2, P3, P4 };
    public enum collisionType { MeInfectOther, Nothing , OtherInfectMe };

    static RuleManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = DestroyOptions.DestroyBehaviour;
        Persist = true;
    }


    [SerializeField] //                            0                        1                  2                     3
    infectationsTypes[] infectionRole = { infectationsTypes.P1, infectationsTypes.P2, infectationsTypes.P3, infectationsTypes.P4 };
    

    public void NextRules() {
        infectionRole.Shuffle();
    }

    public infectationsTypes[] GetRules(){
        return infectionRole;
    }


    public collisionType CompareInfectation(int infect1, int infect2)
    {

        int indexInfect1 = 0;

        if (infect1 == 0 && infect2 == 0) {
            return collisionType.Nothing;
        }

        if (infect1 == 0) {
            return collisionType.OtherInfectMe;
        } else if (infect2 == 0) {
            return collisionType.MeInfectOther;
        }


        for (int i = 1; i < infectionRole.Length; i++) {
            if (infectionRole[i] == (infectationsTypes) infect1) {
                indexInfect1 = i;
                break;
            }
        }

        int nextCheck = (indexInfect1 + 1) % 4;
        int previousCheck = (indexInfect1 - 1) < 0 ? indexInfect1 + 3 : indexInfect1 - 1;


        //Check if I can infect the other
        if (infectionRole[nextCheck] == (infectationsTypes) infect2){
            return collisionType.MeInfectOther;
        }

        if (infectionRole[previousCheck] == (infectationsTypes) infect2)
        {
            return collisionType.OtherInfectMe;
        }

        return collisionType.Nothing;
    }
}