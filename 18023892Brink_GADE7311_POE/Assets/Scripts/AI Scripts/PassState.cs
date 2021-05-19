using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassState : State
{
    TurnSystem ts;
    public DefendState defend;

    public void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();
    }

    public override State RunCurrentState()
    {
        //pass to next player

        ts.CheckHealthBelowZero();
        ts.PassTurnToPlayer();
        return defend;
    }
}
