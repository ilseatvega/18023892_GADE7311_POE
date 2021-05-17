using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    //some parts of this card game followed along with this tutorial, only followed the basics and built my own game from there
    //https://www.youtube.com/playlist?list=PLOoQ0JTWjALQGkiDWw_ws21fanM2za02B
    //ui and icons
    //https://assetstore.unity.com/packages/2d/gui/icons/simple-ui-icons-147101


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

    //card method that can be called to populate the database with the correct cards
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
