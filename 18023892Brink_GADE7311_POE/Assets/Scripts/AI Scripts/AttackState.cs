using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public TurnSystem ts;
    public ThisCard thisCard;

    public GrowState grow;

    public void Start()
    {
        
    }

    public override State RunCurrentState()
    {
        return grow;
    }
}
