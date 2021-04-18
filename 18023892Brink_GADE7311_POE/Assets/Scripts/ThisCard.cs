using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisID;

    public int id;
    public string cardType;
    public string cardname;
    public int cost;
    public int power;

    public Sprite thisSprite;
    public Image thatImage;

    public bool cardBack;
    public static bool staticCardBack;

    public GameObject activeHand;
    public GameObject inactiveHand;

    public int numberOfCardsInDeck;

    private void Start()
    {
        thisCard[0] = CardDB.cardList[thisID];
        numberOfCardsInDeck = PlayerDeck.deckSize;
    }

    private void Update()
    {
        id = thisCard[0].cardID;
        cost = thisCard[0].cost;
        power = thisCard[0].power;
        cardname = thisCard[0].cardName;
        cardType = thisCard[0].cardType;

        thisSprite = thisCard[0].thisCardImage;

        thatImage.sprite = thisSprite;

        staticCardBack = cardBack;

        activeHand = GameObject.Find("Active_Hand");
        if (this.transform.parent == activeHand.transform.parent)
        {
            cardBack = false;
        }

        inactiveHand = GameObject.Find("Inactive_Hand");
        if (this.transform.parent == inactiveHand.transform.parent)
        {
            cardBack = false;
        }

        if (this.tag == "Clone")
        {
            thisCard[0] = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            PlayerDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }
    }
}
