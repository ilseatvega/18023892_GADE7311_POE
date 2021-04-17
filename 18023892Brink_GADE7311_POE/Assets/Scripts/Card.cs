using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public int cardID;
    public string cardType;
    public string cardName;
    //public string cardDescription;
    public int cost;
    public int power;

    public Sprite thisCardImage;

    public Card()
    {

    }

    public Card(int ID, string CardType, string CardName, /*string CardDescription*/ int Cost, int Power, Sprite ThisImage)
    {
        cardID = ID;
        cardType = CardType;
        cardName = CardName;
        //cardDescription = CardDescription;
        cost = Cost;
        power = Power;

        thisCardImage = ThisImage;
    }
}
