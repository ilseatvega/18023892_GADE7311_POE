using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
{
    public GameObject tm;
    public TurnSystem ts;

    public List<Card> thisCard = new List<Card>();
    public int thisID;

    public int id;
    public string cardType;
    public string cardname;
    public string popType;
    public int cost;
    public static int power;
    public static int growth;
    
    public Sprite thisSprite;
    public Image thatImage;

    public bool cardBack;
    public static bool staticCardBack;

    public GameObject activeHand;
    public GameObject inactiveHand;

    public int numberOfCardsInDeck;

    public bool canBeSummoned;
    public bool summoned;
    public GameObject battleZone;

    bool firstUpdate = true;

    private void Start()
    {
        tm = GameObject.FindGameObjectWithTag("Manager");
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();

        thisCard[0] = CardDB.cardList[thisID];
        numberOfCardsInDeck = PlayerDeck.deckSize;
        activeHand = GameObject.FindGameObjectWithTag("AH");
        inactiveHand = GameObject.FindGameObjectWithTag("IH");

        canBeSummoned = false;
        summoned = false;
        

        if (summoned == false && ts.isPlayer1Turn == true)
        {
            if (this.transform.parent == battleZone.transform && CanBeSummoned(1))
            {
                CanBeSummoned(1);
                Summon();

            }
        }
        else if (summoned == false && ts.isPlayer1Turn == false)
        {
            if (this.transform.parent == battleZone.transform && CanBeSummoned(2))
            {
                CanBeSummoned(2);
                Summon2();
            }
        }
    }

    private void Update()
    {
            id = thisCard[0].cardID;
            cost = thisCard[0].cost;
            //Debug.Log($"card summoning cost = {cost}");
            power = thisCard[0].power;
            cardname = thisCard[0].cardName;
            cardType = thisCard[0].cardType;
            popType = thisCard[0].populationType;

            thisSprite = thisCard[0].thisCardImage;
        

            thatImage.sprite = thisSprite;

            staticCardBack = cardBack;

            if (this.transform.parent == activeHand.transform)
            {
                cardBack = false;
            }

            if (this.transform.parent == inactiveHand.transform)
            {
                cardBack = true;
            }

            if (this.tag == "Clone")
            {
                thisCard[0] = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
                numberOfCardsInDeck -= 1;
                PlayerDeck.deckSize -= 1;
                //cardBack = false;
                this.tag = "Untagged";
            }

        if (ts.isPlayer1Turn == true)
        {
            //if (ts.p1currentMana >= cost && summoned == false)
            //{
            //    canBeSummoned = true;
            //}
            if (ts.p1currentMana - cost >= 0 && !summoned)
            {
                canBeSummoned = true;
            }
        }
        else if (ts.isPlayer1Turn == false)
        {
            if (ts.p2currentMana >= cost && summoned == false)
            {
                canBeSummoned = true;
            }
        }
        else
        {
            canBeSummoned = false;
        }
        if (canBeSummoned == true)
        {
            gameObject.GetComponent<Draggable>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Draggable>().enabled = false;
        }


        battleZone = GameObject.Find("Active_Zone");
          
    }

    public bool CanBeSummoned(byte playerID)
    {
        if (playerID == 1)
        {
            if (ts.p1currentMana - cost >= 0)
            {
                //Debug.Log("can be summoned");
                //gameObject.GetComponent<Draggable>().enabled = true;
                return true;
            }
            else
            {
                //Debug.Log("can not be summoned");
                //gameObject.GetComponent<Draggable>().enabled = false;
                return false;
            }
        }
        else if (playerID == 2)
        {
            if (ts.p2currentMana - cost >= 0)
            {
                //Debug.Log("can be summoned");
                
                return true;
            }
            else
            {
                //Debug.Log("can not be summoned");
                return false;
            }
        }
        else
        {
            //Debug.Log("default return");
            throw new CardSpecificException("Cannot check if card can be summoned");
            return false;
        }
    }

    public void Summon()
    {
        //Debug.Log($"Current pre-summon mana: {ts.p1currentMana}");
        ts.RemoveMana(1, cost);
        ts.p1manaText.text = ts.p1currentMana + "/" + ts.p1maxMana;
        WhatCard();
        //Debug.Log($"Current post-nummon mana: {ts.p1currentMana}");
        summoned = true;
    }

    public void Summon2()
    {
        //ts.p2currentMana -= cost;
        ts.RemoveMana(2, cost);
        ts.p2manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
        //Debug.Log($"Current post-nummon mana: {ts.p1currentMana}");
        summoned = true;
    }

    //store health in turn sys
    //what card type is this - when summoning
    public void WhatCard()
    {
        if (ts.isPlayer1Turn == true)
        {
            if (thisCard[0].cardType == "Attack")
            {
                //enable choice canvas
                //village or miliraty attack will happen if other player cant defend
                //card to gy
            }
            else if (thisCard[0].cardType == "Defence" /*&& didAttack == true && thiscard[0].power >= temppower*/)
            {
                //Defence_P1();
            }
            else if (thisCard[0].cardType == "Growth")
            {
                Debug.Log("grwth card");
                Growth_P1();
            }
        }

        if (ts.isPlayer1Turn == false)
        {

        }
    }
    // 
    public void VillageAttack()
    {
        if (ts.isPlayer1Turn == true)
        {
            if (thisCard[0].cardType == "Attack")
            {
                //temp attackholder = power of this card
                //villageattack = true
                //didattack = true
            }
        }
        else if (ts.isPlayer1Turn == false)
        {
            
        }
    }


    //public void Defence_P1()
    //{
    //    if (thisCard[0].cardName == "Village Shield" /*&& villageattack == true && thiscard[0].power >= temppower*/ )
    //    {
    //        //place card in gy
    //    }
    //    else if (thisCard[0].cardName == "Military Shield" /*&& militaryattack == true && thiscard[0].power >= temppower*/)
    //    {
    //        //place card in gy
    //    }
    //    else if (thisCard[0].power == 0/* >= temppower && (villageattack == true || militaryattack == true)*/)
    //    {
    //        //place card in gy
    //    }
    //    else
    //    {
    //        //can't play card
    //        //draggable script disable
    //        if (villageAttack == true)
    //        {
    //            //villageHealth -= tempattack;
    //        }
    //        else if (militaryAttack == true)
    //        {
    //            //militaryHealth -= tempattack;
    //        }
    //    }
    //}

    public void Growth_P1()
    {
        if (thisCard[0].populationType == "V")
        {
            //ts.p1villageText.text = ts.p1villageHealth.ToString();
            ts.p1villageHealth += thisCard[0].growthAmount;
            ts.p1villageText.text = ts.p1villageHealth.ToString();
        }
        else if (thisCard[0].populationType == "M")
        {
            //ts.p1militaryText.text = ts.p1villageHealth.ToString();
            ts.p1militaryHealth += thisCard[0].growthAmount;
            ts.p1militaryText.text = ts.p1villageHealth.ToString();
        }
        else if (thisCard[0].cardName == "Recruit")
        {
            Debug.Log(thisCard[0].growthAmount);
            Debug.Log(ts.p1villageHealth);

            ts.p1villageHealth -= thisCard[0].growthAmount;
            ts.p1militaryHealth += thisCard[0].growthAmount;
        }
        else
        {
            ts.p1villageHealth += thisCard[0].growthAmount;
            ts.p1militaryHealth -= thisCard[0].growthAmount;
        }
    }
}

//custom exception
[Serializable]
public class CardSpecificException : Exception
{
    public CardSpecificException() : base("N.A. card error") { }

    public CardSpecificException(Exception exception):base($"Card error: Unknown {exception}")
    {

    }

    public CardSpecificException(string message):base($"Card Error: {message}")
    {

    }
}