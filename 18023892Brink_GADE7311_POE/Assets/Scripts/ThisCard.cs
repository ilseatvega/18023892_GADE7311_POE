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
    public int power;

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

        if (summoned == false && this.transform.parent == battleZone.transform && ts.isPlayer1Turn == true)
        {
            Summon();
        }
        else if (summoned == false && this.transform.parent == battleZone.transform && ts.isPlayer1Turn == false)
        {
            Summon2();
        }
    }

    private void Update()
    {
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
            if (ts.p1currentMana >= cost && summoned == false)
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

    public void Summon()
    {
        ts.p1currentMana -= cost;
        summoned = true;
    }

    public void Summon2()
    {
        ts.p2currentMana -= cost;
        summoned = true;
    }
}
