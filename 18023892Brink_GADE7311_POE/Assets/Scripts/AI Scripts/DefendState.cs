using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendState : State
{
    public TurnSystem ts;
    public ThisCard thisCard;

    public AttackState attack;

    public void Start()
    {

    }

    public override State RunCurrentState()
    {
        return attack;
    }
}
