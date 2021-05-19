using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendState : State
{
    public TurnSystem ts;

    public AttackState attack;

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
            if (ts.isAttacking == true && 
                inactiveHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p2currentMana &&
                inactiveHand.GetChild(i).GetComponent<ThisCard>().power >= ts.damageHolder &&
                inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Defence")
            {
                //add to list
                cardID.Add(inactiveHand.GetChild(i));
            }
        }

        if (cardID.Count != 0)
        {
            //play the first card
            cardID[0].GetComponent<ThisCard>().DefendAI();
            cardID[0].SetParent(inactiveZone.transform);
            cardID[0].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, cardID[0].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;

            ts.isAttacking = false;
            ts.villageAttack = false;
            ts.militaryAttack = false;
        }

        return attack;
    }
}
