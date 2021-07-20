using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
{
    public TurnSystem ts;

    public List<Card> thisCard = new List<Card>();
    public int thisID;

    public int id;
    public string cardType;
    public string cardname;
    public string popType;
    public int cost;
    public int power;
    public static int growth;
    
    public Sprite thisSprite;
    public Image thatImage;
    public Image displayCard;

    public bool cardBack;
    public static bool staticCardBack;

    public GameObject activeHand;
    public GameObject inactiveHand;

    public int numberOfCardsInDeck;

    public bool canBeSummoned;
    public bool summoned;
    public GameObject battleZone;
    
    public Canvas choiceCanvas;
    

    private void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();
        choiceCanvas = GameObject.Find("ChoiceCanvas").GetComponent<Canvas>();


        thisCard[0] = CardDB.cardList[thisID];
        numberOfCardsInDeck = PlayerDeck.deckSize;
        activeHand = GameObject.FindGameObjectWithTag("AH");
        inactiveHand = GameObject.FindGameObjectWithTag("IH");
        
        canBeSummoned = false;
        summoned = false;
        choiceCanvas.enabled = false;

        displayCard = GameObject.FindGameObjectWithTag("DisplayImage").GetComponent<Image>();
        displayCard.enabled = false;
        //DisplayCard();

    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(this.cardname);
            displayCard.enabled = true;

            thisSprite = this.thatImage.sprite;
            displayCard.sprite = thisSprite;
        }
        if (Input.GetMouseButtonDown(0))
        {
            displayCard.enabled = false;
        }
    }
    private void Update()
    {
        OnMouseDown();

        id = thisCard[0].cardID;
            cost = thisCard[0].cost;
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
            if ((ts.p1currentMana - cost) >= 0 && !summoned)
            {
                canBeSummoned = true;
            }
            else
            {
                canBeSummoned = false;
            }
        }
        else if (ts.isPlayer1Turn == false)
        {
            if ((ts.p2currentMana - cost) >= 0 && !summoned)
            {
                canBeSummoned = true;
            }
            else
            {
                canBeSummoned = false;
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
                Summon();
            }
        }
    }

    public bool CanBeSummoned(byte playerID)
    {
        if (playerID == 1)
        {
            if ((ts.p1currentMana - cost) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (playerID == 2)
        {
            if ((ts.p2currentMana - cost) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            throw new CardSpecificException("Cannot check if card can be summoned");
        }
    }

    public void Summon()
    {
        byte playerID;
        if (ts.isPlayer1Turn == true)
        {
            playerID = 1;
        }
        else playerID = 2;

        ts.RemoveMana(playerID, cost);
        if (playerID == 1)
        {
            ts.p1manaText.text = ts.p1currentMana + "/" + ts.p1maxMana;
        }
        else
        {
            ts.p1manaText.text = ts.p2currentMana + "/" + ts.p2maxMana;
        }
        
        WhatCard();
        summoned = true;
    }
    public void WhatCard()
    {
        //PLAYER1 TURN
        if (ts.isPlayer1Turn == true)
        {
            if (thisCard[0].cardType == "Attack")
            {
                this.transform.SetParent(battleZone.transform);
                ts.isAttacking = true;
                choiceCanvas.enabled = true;
                ts.damageHolder = thisCard[0].power;
            }
            //cannot play defend cards unless an attack has happened
            else if (thisCard[0].cardType == "Defence" && ts.isAttacking == false)
            {
                canBeSummoned = false;
            }
            else if (thisCard[0].cardType == "Defence" && ts.isAttacking == true && thisCard[0].power >= ts.damageHolder)
            {
                Defence_P1();

                ts.isAttacking = false;
                ts.villageAttack = false;
                ts.militaryAttack = false;

            }
            else if (thisCard[0].cardType == "Growth" && ts.isAttacking == true)
            {
                ts.isAttacking = false;
                Growth_P1();
            }
            else if (thisCard[0].cardType == "Growth" && ts.isAttacking == false)
            {
                Growth_P1();
            }
        }

        //PLAYER2 TURN
        if (ts.isPlayer1Turn == false)
        {
            if (thisCard[0].cardType == "Attack")
            {
                this.transform.SetParent(battleZone.transform);
                ts.isAttacking = true;
                choiceCanvas.enabled = true;
                ts.damageHolder = thisCard[0].power;

                Debug.Log(ts.damageHolder);
            }
            //cannot play defend cards unless an attack has happened
            else if (thisCard[0].cardType == "Defence" && ts.isAttacking == false)
            {
                canBeSummoned = false;
            }
            else if (thisCard[0].cardType == "Defence" && ts.isAttacking == true)
            {
                Defence_P2();

                ts.isAttacking = false;
                ts.villageAttack = false;
                ts.militaryAttack = false;

            }
            else if (thisCard[0].cardType == "Growth" && ts.isAttacking == true)
            {
                ts.isAttacking = false;
                Growth_P2();
            }
            else if (thisCard[0].cardType == "Growth" && ts.isAttacking == false)
            {
                Growth_P2();
            }
        }
    }

    public void WhatCardAI()
    {
            if (thisCard[0].cardType == "Attack")
            {
                this.transform.SetParent(battleZone.transform);
                ts.isAttacking = true;
                //choiceCanvas.enabled = true;
                ts.damageHolder = thisCard[0].power;

                Debug.Log(ts.damageHolder);
            }
            //cannot play defend cards unless an attack has happened
            else if (thisCard[0].cardType == "Defence" && ts.isAttacking == false)
            {
                canBeSummoned = false;
            }
            else if (thisCard[0].cardType == "Defence" && ts.isAttacking == true)
            {
                Defence_P2();

                ts.isAttacking = false;
                ts.villageAttack = false;
                ts.militaryAttack = false;

            }
            else if (thisCard[0].cardType == "Growth" && ts.isAttacking == true)
            {
                ts.isAttacking = false;
                Growth_P2();
            }
            else if (thisCard[0].cardType == "Growth" && ts.isAttacking == false)
            {
                Growth_P2();
            }
    }

    // method to control what happens when a village is attacked (village button)
    public void VillageAttack()
    {
        if (ts.isPlayer1Turn == false)
        {
            ts.villageAttack = true;
            ts.militaryAttack = false;

            if (ts.p1villageHealth <= 0)
            {
                ts.p1villageHealth = 0;
            }
            else
            {
                ts.p1villageHealth -= ts.damageHolder;
            }
            
            ts.p1villageText.text = ts.p1villageHealth.ToString();
        }
        else
        {
            ts.villageAttack = true;
            ts.militaryAttack = false;

            if (ts.p2villageHealth <= 0)
            {
                ts.p2villageHealth = 0;
            }
            else
            {
                ts.p2villageHealth -= ts.damageHolder;
            }
            
            ts.p2villageText.text = ts.p2villageHealth.ToString();
        }
        //CheckHealthBelowZero();
        choiceCanvas.enabled = false;
    }
    // method to control what happens when a military is attacked (military button)
    public void MilitaryAttack()
    {
        if (ts.isPlayer1Turn == false)
        {
            ts.villageAttack = false;
            ts.militaryAttack = true;

            if (ts.p1militaryHealth <= 0)
            {
                ts.p1militaryHealth = 0;
            }
            else
            {
                ts.p1militaryHealth -= ts.damageHolder;
            }

            ts.p1militaryText.text = ts.p1militaryHealth.ToString();
        }
        else
        {
            ts.villageAttack = false;
            ts.militaryAttack = true;

            if (ts.p2militaryHealth <= 0)
            {
                ts.p2militaryHealth = 0;
            }
            else
            {
                ts.p2militaryHealth -= ts.damageHolder;
            }
            
            ts.p2militaryText.text = ts.p2militaryHealth.ToString();
        }

        //CheckHealthBelowZero();
        choiceCanvas.enabled = false;
    }

    //the bulk of what happens when a player plays a defend card (depends on what defence card)
    public void Defence_P1()
    {
        this.transform.SetParent(battleZone.transform);
        if (thisCard[0].cardName == "Village Shield" && ts.villageAttack == true && thisCard[0].power >= ts.damageHolder)
        {
            ts.p1villageHealth += ts.damageHolder;
            ts.damageHolder = 0;
            ts.p1villageText.text = ts.p1villageHealth.ToString();
        }
        else if (thisCard[0].cardName == "Military Shield" && ts.militaryAttack == true && thisCard[0].power >= ts.damageHolder)
        {
            ts.p1militaryHealth += ts.damageHolder;
            ts.damageHolder = 0;
            ts.p1militaryText.text = ts.p1militaryHealth.ToString();
        }
        else if (thisCard[0].power >= ts.damageHolder && (thisCard[0].cardName != "Military Shield" || thisCard[0].cardName != "Village Shield"))
        {
            if (ts.villageAttack == true)
            {
                //Debug.Log(ts.damageHolder);
                ts.p1villageHealth += ts.damageHolder;
                ts.damageHolder = 0;
                ts.p1villageText.text = ts.p1villageHealth.ToString();
            }
            else if (ts.militaryAttack == true)
            {
                //Debug.Log(ts.damageHolder);
                ts.p1militaryHealth += ts.damageHolder;
                ts.damageHolder = 0;
                ts.p1militaryText.text = ts.p1militaryHealth.ToString();
            }
        }
        //cant defend
        else
        {
            Debug.Log("couldnt defend");
            canBeSummoned = false;
        }
    }

    //same defence but for player 2
    public void Defence_P2()
    {
        this.transform.SetParent(battleZone.transform);
        if (thisCard[0].cardName == "Village Shield" && ts.villageAttack == true && thisCard[0].power >= ts.damageHolder)
        {
            ts.p2villageHealth += ts.damageHolder;
            ts.damageHolder = 0;
            ts.p2villageText.text = ts.p2villageHealth.ToString();
        }
        else if (thisCard[0].cardName == "Military Shield" && ts.militaryAttack == true && thisCard[0].power >= ts.damageHolder)
        {
            ts.p2militaryHealth += ts.damageHolder;
            ts.damageHolder = 0;
            ts.p2militaryText.text = ts.p2militaryHealth.ToString();
        }
        else if (thisCard[0].power >= ts.damageHolder && (thisCard[0].cardName != "Military Shield" || thisCard[0].cardName != "Village Shield"))
        {
            if (ts.villageAttack == true)
            {
                Debug.Log(ts.damageHolder);
                ts.p2villageHealth += ts.damageHolder;
                ts.damageHolder = 0;
                ts.p2villageText.text = ts.p2villageHealth.ToString();
            }
            else if (ts.militaryAttack == true)
            {
                Debug.Log(ts.damageHolder);
                ts.p2militaryHealth += ts.damageHolder;
                ts.damageHolder = 0;
                ts.p2militaryText.text = ts.p2militaryHealth.ToString();
            }
        }
        //cant defend
        else
        {
            //Debug.Log("couldnt defend");
            canBeSummoned = false;
        }
    }

    //affects health of military/village depending on what type of growth card it is
    public void Growth_P1()
    {
        this.transform.SetParent(battleZone.transform);
        
        if (thisCard[0].populationType == "V")
        {
            ts.p1villageHealth += thisCard[0].growthAmount;
            ts.p1villageText.text = ts.p1villageHealth.ToString();
        }
        else if (thisCard[0].populationType == "M")
        {
            ts.p1militaryHealth += thisCard[0].growthAmount;
            ts.p1militaryText.text = ts.p1militaryHealth.ToString();
        }
        else if (thisCard[0].cardName == "Recruit")
        {

            ts.p1villageHealth -= thisCard[0].growthAmount;
            ts.p1militaryHealth += thisCard[0].growthAmount;

            ts.p1villageText.text = ts.p1villageHealth.ToString();
            ts.p1militaryText.text = ts.p1militaryHealth.ToString();
        }
        //retire
        else
        {
            ts.p1villageHealth += thisCard[0].growthAmount;
            ts.p1militaryHealth -= thisCard[0].growthAmount;

            ts.p1villageText.text = ts.p1villageHealth.ToString();
            ts.p1militaryText.text = ts.p1militaryHealth.ToString();
        }
    }

    //same but for player 2
    public void Growth_P2()
    {
        this.transform.SetParent(battleZone.transform);

        if (thisCard[0].populationType == "V")
        {
            ts.p2villageHealth += thisCard[0].growthAmount;
            ts.p2villageText.text = ts.p2villageHealth.ToString();
        }
        else if (thisCard[0].populationType == "M")
        {
            ts.p2militaryHealth += thisCard[0].growthAmount;
            ts.p2militaryText.text = ts.p2militaryHealth.ToString();
        }
        else if (thisCard[0].cardName == "Recruit")
        {

            ts.p2villageHealth -= thisCard[0].growthAmount;
            ts.p2militaryHealth += thisCard[0].growthAmount;

            ts.p2villageText.text = ts.p2villageHealth.ToString();
            ts.p2militaryText.text = ts.p2militaryHealth.ToString();
        }
        //retire
        else
        {
            ts.p2villageHealth += thisCard[0].growthAmount;
            ts.p2militaryHealth -= thisCard[0].growthAmount;

            ts.p2villageText.text = ts.p2villageHealth.ToString();
            ts.p2militaryText.text = ts.p2militaryHealth.ToString();
        }
    }

    public void GrowAI()
    {
            if (thisCard[0].populationType == "V" && ts.p2villageHealth != 0)
            {
                ts.p2villageHealth += thisCard[0].growthAmount;
                ts.p2villageText.text = ts.p2villageHealth.ToString();
            }
            else if (thisCard[0].populationType == "M" && ts.p2militaryHealth != 0)
            {
                ts.p2militaryHealth += thisCard[0].growthAmount;
                ts.p2militaryText.text = ts.p2militaryHealth.ToString();
            }
            else if (thisCard[0].cardName == "Recruit" && ts.p2villageHealth != 0 && ts.p2militaryHealth != 0)
            {

                ts.p2villageHealth -= thisCard[0].growthAmount;
                ts.p2militaryHealth += thisCard[0].growthAmount;

                ts.p2villageText.text = ts.p2villageHealth.ToString();
                ts.p2militaryText.text = ts.p2militaryHealth.ToString();
            }
            //retire
            else if (ts.p2villageHealth != 0 && ts.p2militaryHealth != 0)
            {
                ts.p2villageHealth += thisCard[0].growthAmount;
                ts.p2militaryHealth -= thisCard[0].growthAmount;

                ts.p2villageText.text = ts.p2villageHealth.ToString();
                ts.p2militaryText.text = ts.p2militaryHealth.ToString();
            }
    }

    public void DefendAI()
    {
        if (thisCard[0].cardName == "Village Shield" && ts.villageAttack == true)
        {
            ts.p2villageHealth += ts.damageHolder;
            ts.damageHolder = 0;
            ts.p2villageText.text = ts.p2villageHealth.ToString();
        }
        else if (thisCard[0].cardName == "Military Shield" && ts.militaryAttack == true)
        {
            ts.p2militaryHealth += ts.damageHolder;
            ts.damageHolder = 0;
            ts.p2militaryText.text = ts.p2militaryHealth.ToString();
        }
        else if ((thisCard[0].cardName != "Military Shield" || thisCard[0].cardName != "Village Shield"))
        {
            if (ts.villageAttack == true)
            {
                Debug.Log(ts.damageHolder);
                ts.p2villageHealth += ts.damageHolder;
                ts.damageHolder = 0;
                ts.p2villageText.text = ts.p2villageHealth.ToString();
            }
            else if (ts.militaryAttack == true)
            {
                Debug.Log(ts.damageHolder);
                ts.p2militaryHealth += ts.damageHolder;
                ts.damageHolder = 0;
                ts.p2militaryText.text = ts.p2militaryHealth.ToString();
            }
        }
    }

    public void AttackAI()
    {
        int random = Random.Range(1,3);
        ts.isAttacking = true;
        ts.damageHolder = thisCard[0].power;

        //village attack
        if (random == 1 && ts.p1villageHealth != 0)
        {
            ts.villageAttack = true;
            ts.militaryAttack = false;

            if (ts.p1villageHealth <= 0)
            {
                ts.p1villageHealth = 0;
            }
            else
            {
                ts.p1villageHealth -= ts.damageHolder;
            }

            ts.p1villageText.text = ts.p1villageHealth.ToString();
        }
        //military attack
        else
        {
            ts.villageAttack = false;
            ts.militaryAttack = true;

            if (ts.p1militaryHealth <= 0)
            {
                ts.p1militaryHealth = 0;
            }
            else
            {
                ts.p1militaryHealth -= ts.damageHolder;
            }

            ts.p1militaryText.text = ts.p1militaryHealth.ToString();
        }
    }
    public void AttackHardAI()
    {
        ts.isAttacking = true;
        ts.damageHolder = thisCard[0].power;

        //village attack
        if (ts.p1villageHealth != 0 && ts.p1villageHealth < ts.p1militaryHealth)
        {
            ts.villageAttack = true;
            ts.militaryAttack = false;

            if (ts.p1villageHealth <= 0)
            {
                ts.p1villageHealth = 0;
            }
            else
            {
                ts.p1villageHealth -= ts.damageHolder;
            }

            ts.p1villageText.text = ts.p1villageHealth.ToString();
        }
        //military attack
        else if(ts.p1militaryHealth != 0 && ts.p1villageHealth > ts.p1militaryHealth)
        {
            ts.villageAttack = false;
            ts.militaryAttack = true;

            if (ts.p1militaryHealth <= 0)
            {
                ts.p1militaryHealth = 0;
            }
            else
            {
                ts.p1militaryHealth -= ts.damageHolder;
            }

            ts.p1militaryText.text = ts.p1militaryHealth.ToString();
        }
    }
    //public void DisplayCard()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Debug.Log(thisCard[0].cardName);
    //        displayCard.enabled = true;

    //        thisSprite = thisCard[0].thisCardImage;
    //        displayCard.sprite = thisSprite;
    //    }
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        displayCard.enabled = false;
    //    }
    //}
    
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