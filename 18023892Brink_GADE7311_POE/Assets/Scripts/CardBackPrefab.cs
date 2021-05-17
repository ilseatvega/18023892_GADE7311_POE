using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackPrefab : MonoBehaviour
{
    public GameObject Deck;
    public GameObject theCard;

    //setting cards to show on deck panel
    void Update()
    {
        Deck = GameObject.Find("DeckPanel");
        theCard.transform.SetParent(Deck.transform);
        theCard.transform.localScale = Vector3.one;
        theCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        theCard.transform.eulerAngles = new Vector3(25, 0, 0);
    }
}
