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
    public string populationType;
    public int cost;
    public int power;

    public Sprite thisCardImage;

    public Card(int ID, string CardType, string CardName, string PopType, int Cost, int Power, Sprite ThisImage)
    {
        cardID = ID;
        cardType = CardType;
        cardName = CardName;
        populationType = PopType;
        cost = Cost;
        power = Power;

        thisCardImage = ThisImage;
    }
}
