using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public TurnSystem ts;
    public State currentState;
    public DefendState defend;

    private void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();
        //urrentState = defend;
    }
    void Update()
    {
        //if pvAI and AI turn
        if (ts.isPVP == false && ts.isPlayer1Turn == false)
        {
            RunFSM();
        }
    }

    private void RunFSM()
    {
        //if not null runcurrentstate
        State nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            //switch next state
            SwitchNextState(nextState);
        }
    }

    //switch current state to the state that is being passed in
    private void SwitchNextState(State nextstate)
    {
        currentState = nextstate;
    }
}
