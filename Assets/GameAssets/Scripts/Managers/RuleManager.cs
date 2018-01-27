using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuleManager : MonoBehaviour
{


    public enum infectationsTypes { red = 1, blue, green, brown };


    int[] diseasesRule = { (int)infectationsTypes.red, (int)infectationsTypes.blue, (int)infectationsTypes.green, (int)infectationsTypes.brown };

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
 


   
    public int CompareInfectation(Infectable infect1, Infectable infect2)
    {

        int d1 = infect1.infectedBy.GetComponent<Player>().playerIndex;
        int d2 = infect2.infectedBy.GetComponent<Player>().playerIndex;
        int d1Pos = 0;
        int d2Pos = 0;

        for (int i = 0; i < diseasesRule.Length; i++) {
            if (diseasesRule[i] == d1) {
                d1Pos = i;
            }
        
            if (diseasesRule[i] == d2)
            {
                d2Pos = i;
            }
        }

        if (d1Pos +1 == d2Pos|| (d1Pos==3 && d2Pos==0)) {
            return 1;
        }

        if (d2Pos + 1 == d1Pos || (d2Pos == 3 && d1Pos == 0))
        {
            return -1;
        }

        return 0;
    }
}