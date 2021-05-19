using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowState : State
{
    public TurnSystem ts;

    public PassState pass;

    bool hasMana;

    private RectTransform inactiveZone;
    private RectTransform inactiveHand;
    private int count;
    
    List<Transform> cardID;

    public void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();

        inactiveHand = GameObject.FindGameObjectWithTag("IH").GetComponent<RectTransform>();
        inactiveZone = GameObject.FindGameObjectWithTag("IZ").GetComponent<RectTransform>();
    }

    public override State RunCurrentState()
    {
        count = inactiveHand.transform.childCount;

        cardID = new List<Transform>(count);

        //loop through all cards to see if requirements met
        for (int i = 0; i < count; i++)
        {
            //if cost of card is less than or equal to mana count and growth card
            if (inactiveHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p2currentMana && 
                inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Growth")
            {
                //add to list
                cardID.Add(inactiveHand.GetChild(i));
            }
        }

        //if list not empty
        if (cardID.Count != 0)
        {
            //play the first card
            cardID[0].GetComponent<ThisCard>().GrowAI();
            cardID[0].SetParent(inactiveZone.transform);
            cardID[0].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, cardID[0].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
            //keep playing growth cards until out of mana
            return this;
        }
        
        //move to pass state
        return pass;
    }
    
}
