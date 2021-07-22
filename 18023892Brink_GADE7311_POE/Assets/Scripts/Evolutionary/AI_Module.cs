using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class AI_Module : MonoBehaviour
{
    public TurnSystem ts;
    public TrainingManager tm;
    
    bool hasMana;

    //p2
    private RectTransform inactiveZone;
    private RectTransform inactiveHand;
    //p1
    private RectTransform activeZone;
    private RectTransform activeHand;

    private int count;
    private int validCount;
    private int costCount;
    private int contains;

    List<Transform> validCards;
    List<float> weightList;
    List<string> popType;

    private Canvas winCanvas;

    // Start is called before the first frame update
    void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();
        tm = GameObject.FindGameObjectWithTag("Manager").GetComponent<TrainingManager>();

        inactiveHand = GameObject.FindGameObjectWithTag("IH").GetComponent<RectTransform>();
        inactiveZone = GameObject.FindGameObjectWithTag("IZ").GetComponent<RectTransform>();

        activeHand = GameObject.FindGameObjectWithTag("AH").GetComponent<RectTransform>();
        activeZone = GameObject.FindGameObjectWithTag("AZ").GetComponent<RectTransform>();

        winCanvas = GameObject.Find("WinCanvas").GetComponent<Canvas>();

        if (ts.isPlayer1Turn == true)
        {
            Player1Turn();
        }
        else
        {
            Player2Turn();
        }
    }

    private void DefendP1()
    {
        count = activeHand.transform.childCount;
        validCards = new List<Transform>(count);
        weightList = new List<float>(validCount);

        //disable dragging cards - only ai can play
        for (int i = 0; i < count; i++)
        {
            if (activeHand.GetChild(i))
            {
                if (activeHand.GetChild(i).GetComponent<Draggable>())
                {
                    activeHand.GetChild(i).GetComponent<Draggable>().Disable();
                }
            }
        }

        //loop through all cards to see if requirements met - has mana and is defence card
        for (int i = 0; i < count; i++)
        {
            //if cost of card is less than or equal to mana count and growth card and has enough power to def att
            if (activeHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p1currentMana &&
                activeHand.GetChild(i).GetComponent<ThisCard>().cardType == "Defence" &&
                activeHand.GetChild(i).GetComponent<ThisCard>().power >= ts.damageHolder)
            {
                //add to list
                validCards.Add(activeHand.GetChild(i));
            }
        }

        //if there are cards to play, play the card w the highest weighting
        if (validCards.Count != 0)
        {
            validCount = validCards.Count;

            //add weight of each card to list  [1, 2, 3]  [0.5, 0.2, 0.3] <---- just a visual rep of both lists and positions for me
            foreach (Transform cards in validCards)
            {
                weightList.Add(tm.GetWeightFromID(cards.GetComponent<ThisCard>().cardname));
            }
            //finding index in weightlist w highest weight
            int index = weightList.FindIndex(a => a == weightList.Max());

            //add to track card
            tm.AddCardToTCard(validCards[index].gameObject,
                              validCards[index].GetComponent<ThisCard>().cardname,
                              validCards[index].GetComponent<ThisCard>().cardType);

            int defended = 0;
            //if (validCards[index].GetComponent<ThisCard>().power >= ts.damageHolder)
            //{
            defended = ts.damageHolder;
            //}

            //set the dmg defended amount
            tm.SetDmgDefended(validCards[index].gameObject,
                              defended);
            //if defend saved village population from dying
            if (ts.villageAttack == true && (ts.p1villageHealth - ts.damageHolder) <= 0)
            {
                tm.SavedPop(validCards[index].gameObject, true);
            }
            else if (ts.militaryAttack == true && (ts.p1militaryHealth - ts.damageHolder) <= 0)
            {
                tm.SavedPop(validCards[index].gameObject, true);
            }

            Debug.Log("defending p1");
            validCards[index].GetComponent<ThisCard>().DefendAI_P1();
            validCards[index].SetParent(activeZone.transform);
            validCards[index].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(1, validCards[index].GetComponent<ThisCard>().cost);
            ts.p1manaText.text = ts.p1currentMana + "/" + ts.p1maxMana;
            ts.isAttacking = false;
            ts.villageAttack = false;
            ts.militaryAttack = false;

            validCards.Clear();
            AttackOrGrowP1();
        }
        //no valid cards
        else
        {
            AttackOrGrowP1();
        }
    }
    private void DefendP2()
    {
        count = inactiveHand.transform.childCount;
        validCards = new List<Transform>(count);
        weightList = new List<float>(validCount);

        //loop through all cards to see if requirements met - has mana and is defence card
        for (int i = 0; i < count; i++)
        {
            //if cost of card is less than or equal to mana count and growth card and has enough power to def att
            if (inactiveHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p2currentMana &&
                inactiveHand.GetChild(i).GetComponent<ThisCard>().cardType == "Defence" &&
                inactiveHand.GetChild(i).GetComponent<ThisCard>().power >= ts.damageHolder)
            {
                //add to list
                validCards.Add(inactiveHand.GetChild(i));
            }
        }

        //if there are cards to play, play the card w the highest weighting
        if (validCards.Count != 0)
        {
            validCount = validCards.Count;

            //add weight of each card to list  [1, 2, 3]  [0.5, 0.2, 0.3] <---- just a visual rep of both lists and positions for me
            foreach (Transform cards in validCards)
            {
                weightList.Add(tm.GetWeightFromID(cards.GetComponent<ThisCard>().cardname));
            }
            //finding index in weightlist w highest weight
            int index = weightList.FindIndex(a => a == weightList.Max());

            //add to track card
            tm.AddCardToTCard(validCards[index].gameObject,
                              validCards[index].GetComponent<ThisCard>().cardname,
                              validCards[index].GetComponent<ThisCard>().cardType);

            int defended=0;
            //if (validCards[index].GetComponent<ThisCard>().power >= ts.damageHolder)
            //{
                defended = ts.damageHolder;
            //}

            //set the dmg defended amount
            tm.SetDmgDefended(validCards[index].gameObject,
                              defended);
            //if defend saved village population from dying
            if (ts.villageAttack == true && (ts.p2villageHealth - ts.damageHolder) <= 0)
            {
                tm.SavedPop(validCards[index].gameObject, true);
            }
            else if (ts.militaryAttack == true && (ts.p2militaryHealth - ts.damageHolder) <= 0)
            {
                tm.SavedPop(validCards[index].gameObject, true);
            }

            Debug.Log("defending 2");

            validCards[index].GetComponent<ThisCard>().DefendAI();
            validCards[index].SetParent(inactiveZone.transform);
            validCards[index].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, validCards[index].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
            ts.isAttacking = false;
            ts.villageAttack = false;
            ts.militaryAttack = false;

            validCards.Clear();
            AttackOrGrowP2();
        }
        //no valid cards
        else
        {
            AttackOrGrowP2();
        }
    }

    private void AttackOrGrowP1()
    {
        count = activeHand.transform.childCount;
        validCards = new List<Transform>(count);
        weightList = new List<float>(validCount);

        //disable dragging cards - only ai can play
        for (int i = 0; i < count; i++)
        {
            if (activeHand.GetChild(i))
            {
                if (activeHand.GetChild(i).GetComponent<Draggable>())
                {
                    activeHand.GetChild(i).GetComponent<Draggable>().Disable();
                }
            }
        }

        //loop through all cards to see if requirements met - has mana and is defence card
        for (int i = 0; i < count; i++)
        {
            //if cost of card is less than or equal to mana count and growth card or att card
            if (activeHand.GetChild(i).GetComponent<ThisCard>().cost <= ts.p1currentMana &&
               (activeHand.GetChild(i).GetComponent<ThisCard>().cardType == "Attack" || activeHand.GetChild(i).GetComponent<ThisCard>().cardType == "Growth"))
            {
                //add to list
                validCards.Add(activeHand.GetChild(i));
                //Debug.Log(validCards[0].GetComponent<ThisCard>().cardname);
            }
        }

        //if there are cards to play, play the card w the highest weighting
        if (validCards.Count != 0)
        {
            validCount = validCards.Count;

            //add weight of each card to list  [1, 2, 3]  [0.5, 0.2, 0.3]
            foreach (Transform cards in validCards)
            {
                weightList.Add(tm.GetWeightFromID(cards.GetComponent<ThisCard>().cardname));
            }
            //finding index in weightlist w highest weight
            int index = weightList.FindIndex(a => a == weightList.Max());

            //add to track card
            tm.AddCardToTCard(validCards[index].GetComponent<ThisCard>().gameObject,
                              validCards[index].GetComponent<ThisCard>().cardname,
                              validCards[index].GetComponent<ThisCard>().cardType);

            //------------IF ATTACK TYPE-----
            if (validCards[index].GetComponent<ThisCard>().cardType == "Attack")
            {
                ts.isAttacking = true;

                ts.damageHolder = validCards[index].GetComponent<ThisCard>().power;

                //set the dmg defended amount
                tm.SetDamageDealt(validCards[index].gameObject, ts.damageHolder);

                if (ts.p2militaryHealth == 0)
                {
                    if (ts.p2villageHealth - ts.damageHolder <= 0)
                    {
                        tm.WonTheGame(validCards[index].gameObject, true);
                    }
                }
                else if (ts.p2villageHealth == 0)
                {
                    if (ts.p2militaryHealth - ts.damageHolder <= 0)
                    {
                        tm.WonTheGame(validCards[index].gameObject, true);
                    }
                }

                validCards[index].GetComponent<ThisCard>().AttackAI_P1();
            }
            //--------------------GROWTH TYPE------
            else
            {
                //if village card can grow the village while the villagehealth is low (below 100)
                if (validCards[index].GetComponent<ThisCard>().popType == "V" && ts.p1villageHealth >= 100)
                {
                    tm.GrowUnderHealth(validCards[index].gameObject, true);
                }
                //same but for military
                else if (validCards[index].GetComponent<ThisCard>().popType == "M" && ts.p1militaryHealth >= 100)
                {
                    tm.GrowUnderHealth(validCards[index].gameObject, true);
                }

                validCards[index].GetComponent<ThisCard>().GrowAI_P1();
            }

            validCards[index].SetParent(activeZone.transform);
            validCards[index].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(1, validCards[index].GetComponent<ThisCard>().cost);
            ts.p1manaText.text = ts.p1currentMana + "/" + ts.p1maxMana;

            validCards.Clear();
            AttackOrGrowP1();
        }
        //no valid cards
        else
        {
            Pass();
        }
    }
    private void AttackOrGrowP2()
    {
        count = inactiveHand.transform.childCount;
        validCards = new List<Transform>(count);
        weightList = new List<float>(validCount);

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
            validCount = validCards.Count;

            //add weight of each card to list  [1, 2, 3]  [0.5, 0.2, 0.3]
            foreach (Transform cards in validCards)
            {
                weightList.Add(tm.GetWeightFromID(cards.GetComponent<ThisCard>().cardname));
            }
            //finding index in weightlist w highest weight
            int index = weightList.FindIndex(a => a == weightList.Max());

            //add to track card
            tm.AddCardToTCard(validCards[index].GetComponent<ThisCard>().gameObject,
                              validCards[index].GetComponent<ThisCard>().cardname,
                              validCards[index].GetComponent<ThisCard>().cardType);

            //------------IF ATTACK TYPE-----
            if (validCards[index].GetComponent<ThisCard>().cardType == "Attack")
            {
                ts.isAttacking = true;

                ts.damageHolder = validCards[index].GetComponent<ThisCard>().power;

                //set the dmg defended amount
                tm.SetDamageDealt(validCards[index].gameObject, ts.damageHolder);

                if (ts.p1militaryHealth == 0)
                {
                    if (ts.p1villageHealth - ts.damageHolder <= 0)
                    {
                        tm.WonTheGame(validCards[index].gameObject, true);
                    }
                }
                else if (ts.p1villageHealth == 0)
                {
                    if (ts.p1militaryHealth - ts.damageHolder <= 0)
                    {
                        tm.WonTheGame(validCards[index].gameObject, true);
                    }
                }

                validCards[index].GetComponent<ThisCard>().AttackAI();
            }
            //--------------------GROWTH TYPE------
            else
            {
                //if village card can grow the village while the villagehealth is low (below 100)
                if (validCards[index].GetComponent<ThisCard>().popType == "V" && ts.p2villageHealth >= 100)
                {
                    tm.GrowUnderHealth(validCards[index].gameObject, true);
                }
                //same but for military
                else if (validCards[index].GetComponent<ThisCard>().popType == "M" && ts.p2militaryHealth >= 100)
                {
                    tm.GrowUnderHealth(validCards[index].gameObject, true);
                }

                validCards[index].GetComponent<ThisCard>().GrowAI();
            }
            
            validCards[index].SetParent(inactiveZone.transform);
            validCards[index].GetComponent<ThisCard>().cardBack = false;

            ts.RemoveMana(2, validCards[index].GetComponent<ThisCard>().cost);
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;

            validCards.Clear();
            AttackOrGrowP2();
        }
        //no valid cards
        else
        {
            Pass();
        }
    }

    private void Pass()
    {
        //pass to P1
        if (!ts.isPlayer1Turn)
        {
            ts.CheckHealthBelowZero();

            ts.p1Turn += 1;
            if (ts.p1maxMana != 10)
            {
                ts.p1maxMana += 1;
            }
            ts.p1currentMana = ts.p1maxMana;

            ts.p1manaText.text = ts.p1currentMana + "/" + ts.p1maxMana;
            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;

            //ts.startTurn = true;
            ts.isPlayer1Turn = true;
            TurnSystem.startTurn = true;
            Player1Turn();
        }
        //pass to P2
        else
        {
            ts.CheckHealthBelowZero();
            ts.p2Turn += 1;
            if (ts.p2maxMana != 10)
            {
                ts.p2maxMana += 1;
            }
            ts.p2currentMana = ts.p2maxMana;

            ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
            //ts.p1manaText.text = ts.p1currentMana + "/" + ts.p1maxMana;
            
            ts.isPlayer1Turn = false;
            TurnSystem.startTurn = true;
            Player2Turn();
        }
    }

    private void Player1Turn()
    {
        StartCoroutine(P1WaitTime());
    }
    private void Player2Turn()
    {
        StartCoroutine(P2WaitTime());
    }
    //
    private void GameEnded()
    {
        if (winCanvas.enabled == true)
        {
            int activeCount= activeZone.transform.childCount;
            int inactiveCount = inactiveZone.transform.childCount;

            //remove all active cards
            for (int i = 0; i < activeCount; i++)
            {
                tm.RemoveCardFromTCard(activeZone.GetChild(i).gameObject);
            }
            //remove all inactive cards
            for (int i = 0; i < inactiveCount; i++)
            {
                tm.RemoveCardFromTCard(inactiveZone.GetChild(i).gameObject);
            }

            tm.SaveTrainingData();
            //reload scene to keep training
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator P1WaitTime()
    {
        yield return new WaitForSeconds(2f);
        //defend
        if (ts.isAttacking == true)
        {
            DefendP1();
        }
        //attack or grow
        else if (ts.isAttacking == false)
        {
            AttackOrGrowP1();
        }
        //check if game ended
        GameEnded();
        ts.isPlayer1Turn = false;
    }
    IEnumerator P2WaitTime()
    {
        yield return new WaitForSeconds(2f);
        //defend
        if (ts.isAttacking == true)
        {
            DefendP2();
        }
        //attack or grow
        else if (ts.isAttacking == false)
        {
            AttackOrGrowP2();
        }
        //check if game ended
        GameEnded();
        ts.isPlayer1Turn = true;
    }
}
