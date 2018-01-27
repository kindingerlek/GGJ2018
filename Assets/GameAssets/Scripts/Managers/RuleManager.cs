using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuleManager : MonoBehaviour
{


    public enum infectationsTypes { red = 1, blue, green, brown };

    [SerializeField]
    int[] diseasesRule = { 0, (int)infectationsTypes.red, (int)infectationsTypes.blue, (int)infectationsTypes.green, (int)infectationsTypes.brown };


    static RuleManager() {
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextRules() {
        diseasesRule.Shuffle();
    }

    public int[] GetRules(){
        return diseasesRule;
    }


    public int CompareInfectation(int infect1, int infect2)
    {

        int d1Pos = 1;
        int d2Pos = 1;

        if (infect1 == 0 && infect2 == 0) {
            return 0;
        }
        for (int i = 1; i < diseasesRule.Length; i++) {
            if (diseasesRule[i] == infect1) {
                d1Pos = i;
                continue;
            }
        
            if (diseasesRule[i] == infect2)
            {
                d2Pos = i;
            }
        }

        if (d1Pos +1 == d2Pos|| (d1Pos==4 && d2Pos==1) ||infect2 == 0) {
            return 1;
        }

        if (d2Pos + 1 == d1Pos || (d2Pos == 4 && d1Pos == 1) || infect1 == 0)
        {
            return -1;
        }

        return 0;
    }
}