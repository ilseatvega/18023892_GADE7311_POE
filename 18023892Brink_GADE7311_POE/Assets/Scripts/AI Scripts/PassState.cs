using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassState : State
{
    TurnSystem ts;
    public DefendState defend;

    //public setDifficulty aiDifficulty;

    public void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();

        //aiDifficulty = GameObject.FindGameObjectWithTag("ScreenManager").GetComponent<StartScreen>().AI_Diff;
    }

    public override State RunCurrentState()
    {
        //pass to next player
        ts.CheckHealthBelowZero();
        ts.PassTurnToPlayer();
        return defend;
    }
}
