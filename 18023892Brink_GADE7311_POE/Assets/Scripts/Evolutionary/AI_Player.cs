using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_Player : MonoBehaviour
{
    public TurnSystem ts;
    public TrainingManager tm;

    private RectTransform inactiveZone;
    private RectTransform inactiveHand;

    private int count;

    private string trainingPath;

    List<Transform> validCards;

    // Start is called before the first frame update
    void Start()
    {
        trainingPath = Application.dataPath + @"\ObjectData\TextFiles\TrainingData.txt";

        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();
        tm = GameObject.FindGameObjectWithTag("Manager").GetComponent<TrainingManager>();

        inactiveHand = GameObject.FindGameObjectWithTag("IH").GetComponent<RectTransform>();
        inactiveZone = GameObject.FindGameObjectWithTag("IZ").GetComponent<RectTransform>();
    }

    public void PlayHighestCard()
    {
        if (ts.isAttacking == true)
        {
            count = inactiveHand.transform.childCount;

            validCards = new List<Transform>(count);

            //loop through all cards to see if requirements met
            for (int i = 0; i < count; i++)
            {
                //if cost of card is less than or equal to mana count and growth card
                if (inactiveHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p2currentMana &&
                    inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Defence")
                {
                    //add to list
                    validCards.Add(inactiveHand.GetChild(i));
                }
            }
            //if list not empty
            if (validCards.Count != 0)
            {
                HighestCard cardToPlay = new HighestCard();
                cardToPlay = tm.findHighestWeight(validCards);

                //play the first card
                validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().DefendAI();
                validCards[cardToPlay.cardIndex].SetParent(inactiveZone.transform);
                validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().cardBack = false;

                ts.RemoveMana(2, validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().cost);
                ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
            }
            validCards.Clear();
            AttackOrGrow();
        }
        //play attacking or growth
        else
        {
            AttackOrGrow();
            ts.PassTurnToPlayer();
        }
    }

    public void AttackOrGrow()
    {
        count = inactiveHand.transform.childCount;
        validCards = new List<Transform>(count);

        //loop through all cards to see if requirements met - has mana and is defence card
        for (int i = 0; i < count; i++)
        {
            //if cost of card is less than or equal to mana count and growth card or att card
            if (inactiveHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p2currentMana &&
               (inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Attack" || inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Growth"))
            {
                //add to list
                validCards.Add(inactiveHand.GetChild(i));
            }
        }

        //if there are cards to play, play the card w the highest weighting
        if (validCards.Count != 0)
        {
            Debug.Log(validCards[0]);
            HighestCard cardToPlay = new HighestCard();
            cardToPlay = tm.findHighestWeight(validCards);

            //------------IF ATTACK TYPE-----
            if (validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().cardType == "Attack")
            {
                ts.isAttacking = true;

                ts.damageHolder = validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().power;

                //set the dmg defended amount
                tm.SetDamageDealt(validCards[cardToPlay.cardIndex].gameObject, ts.damageHolder);

                if (ts.p1militaryHealth == 0)
                {
                    if (ts.p1villageHealth - ts.damageHolder <= 0)
                    {
                        tm.WonTheGame(validCards[cardToPlay.cardIndex].gameObject, true);
                    }
                }
                else if (ts.p1villageHealth == 0)
                {
                    if (ts.p1militaryHealth - ts.damageHolder <= 0)
                    {
                        tm.WonTheGame(validCards[cardToPlay.cardIndex].gameObject, true);
                    }
                }

                validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().AttackAI();
            }
            //--------------------GROWTH TYPE------
            else
            {
                //if village card can grow the village while the villagehealth is low (below 100)
                if (validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().popType == "V" && ts.p2villageHealth >= 100)
                {
                    tm.GrowUnderHealth(validCards[cardToPlay.cardIndex].gameObject, true);
                }
                //same but for military
                else if (validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().popType == "M" && ts.p2militaryHealth >= 100)
                {
                    tm.GrowUnderHealth(validCards[cardToPlay.cardIndex].gameObject, true);
                }

                validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().GrowAI();
            }

            validCards[cardToPlay.cardIndex].SetParent(inactiveZone.transform);
            validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, validCards[cardToPlay.cardIndex].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;

            validCards.Clear();
            AttackOrGrow();
        }
        //no valid cards
        else
        {
            ts.PassTurnToPlayer();
        }
    }
}
