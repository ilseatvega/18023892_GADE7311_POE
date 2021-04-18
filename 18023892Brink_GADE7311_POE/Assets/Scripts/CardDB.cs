using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDB : MonoBehaviour
{
    public static List<Card> cardList = new List<Card> ();

    private void Awake()
    {
        //id, type, name, villge or military, mana, power, load sprite
        //V = village, M = military, S = swap, OR = choose, "" = N/A

        //ATTACK
        cardList.Add(new Card(1, "Attack", "Arch-Angel", "OR" , 2, 30, Resources.Load <Sprite>("Arch-Angel")));
        cardList.Add(new Card(2, "Attack", "Ground Cover", "OR", 1, 20, Resources.Load<Sprite>("Ground Cover")));
        cardList.Add(new Card(3, "Attack", "Missile Mission", "OR", 8, 100, Resources.Load<Sprite>("Missile Mission")));
        cardList.Add(new Card(4, "Attack", "Silent Night", "OR", 4, 50, Resources.Load<Sprite>("Silent Night")));
        cardList.Add(new Card(5, "Attack", "Spoilt Rotten", "OR", 2, 40, Resources.Load<Sprite>("Spoilt Rotten")));
        cardList.Add(new Card(6, "Attack", "Unconventional Killers", "OR", 6, 80, Resources.Load<Sprite>("Unconventional Killers")));
        cardList.Add(new Card(7, "Attack", "XX", "OR", 3, 60, Resources.Load<Sprite>("XX")));

        //DEFENCE
        cardList.Add(new Card(8, "Defence", "Builder", "", 2, 40, Resources.Load<Sprite>("Builder")));
        cardList.Add(new Card(9, "Defence", "In Formation", "", 6, 80, Resources.Load<Sprite>("In Formation")));
        cardList.Add(new Card(10, "Defence", "Military Shield", "", 8, 100, Resources.Load<Sprite>("Military Shield")));
        cardList.Add(new Card(11, "Defence", "Shield Up", "", 1, 20, Resources.Load<Sprite>("Shield Up")));
        cardList.Add(new Card(12, "Defence", "Ultimate", "", 5, 120, Resources.Load<Sprite>("Ultimate")));
        cardList.Add(new Card(13, "Defence", "Under Cover", "", 3, 60, Resources.Load<Sprite>("Under Cover")));
        cardList.Add(new Card(14, "Defence", "Village Shield", "", 8, 100, Resources.Load<Sprite>("VS")));

        //GROWTH
        cardList.Add(new Card(15, "Growth", "For Honour", "M", 2, 0, Resources.Load<Sprite>("FH")));
        cardList.Add(new Card(16, "Growth", "Fertile Years", "V", 3, 0, Resources.Load<Sprite>("FY")));
        cardList.Add(new Card(17, "Growth", "Guts and Glory", "M", 3, 0, Resources.Load<Sprite>("G&G")));
        cardList.Add(new Card(18, "Growth", "The More The Merrier", "V", 2, 0, Resources.Load<Sprite>("Merry")));
        cardList.Add(new Card(19, "Growth", "Recruit", "S", 1, 0, Resources.Load<Sprite>("Recruit")));
        cardList.Add(new Card(20, "Growth", "Retire", "S", 1, 0, Resources.Load<Sprite>("Retire")));
    }
}
