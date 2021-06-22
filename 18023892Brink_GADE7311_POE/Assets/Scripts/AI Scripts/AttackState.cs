using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackState : State
{
    public TurnSystem ts;

    public PassState pass;

    private RectTransform inactiveZone;
    private RectTransform inactiveHand;
    private int count;
    private int costCount;
    private int index;

    List<Transform> cardID;
    List<int> cardCost;

    public setDifficulty aiDifficulty;

    public void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();

        inactiveHand = GameObject.FindGameObjectWithTag("IH").GetComponent<RectTransform>();
        inactiveZone = GameObject.FindGameObjectWithTag("IZ").GetComponent<RectTransform>();

        aiDifficulty = GameObject.FindGameObjectWithTag("ScreenManager").GetComponent<StartScreen>().AI_Diff;
    }

    public override State RunCurrentState()
    {
        count = inactiveHand.transform.childCount;

        cardID = new List<Transform>(count);

        //loop through all cards to see if requirements met
        for (int i = 0; i < count; i++)
        {
            //if cost of card is less than or equal to mana count and growth card
            if (inactiveHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p2currentMana && inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Attack")
            {
                //add to list
                cardID.Add(inactiveHand.GetChild(i));
            }
        }

        if (cardID.Count != 0)
        {
            //play the first card
            cardID[0].GetComponent<ThisCard>().AttackAI();
            cardID[0].SetParent(inactiveZone.transform);
            cardID[0].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, cardID[0].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;

            return this;
        }
        //---------------HARD MODE-------------
        else if (cardID.Count != 0 && aiDifficulty == setDifficulty.hard)
        {
            costCount = cardID.Count;
            cardCost = new List<int>(costCount);
            
            //loop through cards and add all valid card's costs to a new list
            for (int i = 0; i < costCount; i++)
            {
                if (cardID[i] != null)
                {
                    cardCost.Add(cardID[i].GetComponent<ThisCard>().cost);
                }
            }

            //the card that had the lowest value is played - the position of the cardID is determined by the min value in the cardcost list

            int index = cardCost.FindIndex(a => a == cardCost.Min());

            cardID[index].GetComponent<ThisCard>().AttackHardAI();
            cardID[index].SetParent(inactiveZone.transform);
            cardID[index].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, cardID[index].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;

            return this;
        }

        return pass;
    }
}
