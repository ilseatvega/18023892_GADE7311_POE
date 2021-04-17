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

    private void Start()
    {
        thisCard[0] = CardDB.cardList[thisID];
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
    }
}
