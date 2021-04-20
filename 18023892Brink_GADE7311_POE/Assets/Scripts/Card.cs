using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    //some parts of this card game followed along with this tutorial, only followed the basics and built my own game from there
    //https://www.youtube.com/playlist?list=PLOoQ0JTWjALQGkiDWw_ws21fanM2za02B


    public int cardID;
    public string cardType;
    public string cardName;
    public string populationType;
    public int cost;
    public int power;
    
    public int growthAmount;

    public Sprite thisCardImage;

    //public Card()
    //{

    //}

    public Card(int ID, string CardType, string CardName, string PopType, int Cost, int Power, Sprite ThisImage, int Growth)
    {
        cardID = ID;
        cardType = CardType;
        cardName = CardName;
        populationType = PopType;
        cost = Cost;
        power = Power;

        thisCardImage = ThisImage;
        
        growthAmount = Growth;
    }
}
