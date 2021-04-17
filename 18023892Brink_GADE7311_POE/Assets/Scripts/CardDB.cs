using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDB : MonoBehaviour
{
    public static List<Card> cardList = new List<Card> ();

    private void Awake()
    {
        cardList.Add(new Card(0, "Attack", "Arch-Angel", 2, 30, Resources.Load <Sprite>("Zent Front")));
        cardList.Add(new Card(0, "Attack", "Bonk", 2, 30, Resources.Load<Sprite>("Zent Front")));
        cardList.Add(new Card(0, "Attack", "Boop", 2, 30, Resources.Load<Sprite>("Zent Front")));
        cardList.Add(new Card(0, "Attack", "Stinky", 2, 30, Resources.Load<Sprite>("Zent Front")));
        cardList.Add(new Card(0, "Attack", "Yes", 2, 30, Resources.Load<Sprite>("Zent Front")));

    }
}
