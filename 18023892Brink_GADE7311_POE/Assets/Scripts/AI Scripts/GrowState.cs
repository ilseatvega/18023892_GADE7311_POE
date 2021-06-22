using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrowState : State
{
    public TurnSystem ts;

    public AttackState attack;

    bool hasMana;

    private RectTransform inactiveZone;
    private RectTransform inactiveHand;
    private int count;
    private int costCount;
    private int popTypeCount;
    private int index;
    private int contains;

    List<Transform> cardID;
    List<int> cardCost;
    List<string> popType;

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
            if (inactiveHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p2currentMana && 
                inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Growth")
            {
                //add to list
                cardID.Add(inactiveHand.GetChild(i));
            }
        }

        //if list not empty
        if (cardID.Count != 0 && aiDifficulty==setDifficulty.easy)
        {
            //play the first card
            cardID[0].GetComponent<ThisCard>().GrowAI();
            cardID[0].SetParent(inactiveZone.transform);
            cardID[0].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, cardID[0].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
            //keep playing growth cards until out of mana
            //return this;
        }
        //-----------------HARD MODE-----------------------
        else if (cardID.Count != 0 && aiDifficulty == setDifficulty.hard)
        {
            popTypeCount = cardID.Count;
            cardCost = new List<int>(costCount);
            popType = new List<string>(popTypeCount);

            //loop through cards and add all valid card's pop type to a new list - are they village or military growth?
            for (int i = 0; i < popTypeCount; i++)
            {
                if (cardID[i] != null)
                {
                    //Debug.Log(cardID[0]);
                    popType.Add(cardID[i].GetComponent<ThisCard>().popType);
                    //Debug.Log(popType[0]);
                }
            }
            
            if (ts.p2militaryHealth >= ts.p2villageHealth)
            {
                costCount = popType.Count;
                contains = popType.FindIndex(a => a.Contains("V"));
                if (contains != -1)
                {
                    for (int i = 0; i < costCount; i++)
                    {
                        if (cardID[i] != null)
                        {
                            cardCost.Add(cardID[i].GetComponent<ThisCard>().cost);
                        }
                    }
                    //Debug.Log(cardCost[0]);
                }
                else
                {
                    for (int i = 0; i < costCount; i++)
                    {
                        if (cardID[i] != null)
                        {
                            cardCost.Add(cardID[i].GetComponent<ThisCard>().cost);
                        }
                    }
                    //Debug.Log(cardCost[0]);
                }
            }
            else if (ts.p2villageHealth >= ts.p2militaryHealth)
            {
                contains = popType.FindIndex(a => a.Contains("M"));
                if (contains != -1)
                {
                    for (int i = 0; i < costCount; i++)
                    {
                        if (cardID[i] != null)
                        {
                            cardCost.Add(cardID[i].GetComponent<ThisCard>().cost);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < costCount; i++)
                    {
                        if (cardID[i] != null)
                        {
                            cardCost.Add(cardID[i].GetComponent<ThisCard>().cost);
                        }
                    }
                }
            }
            
            int index = cardCost.FindIndex(a => a == cardCost.Min());

            cardID[index].GetComponent<ThisCard>().GrowAI();
            cardID[index].SetParent(inactiveZone.transform);
            cardID[index].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, cardID[index].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
        }
        
        //move to attack state
        return attack;
    }
}
