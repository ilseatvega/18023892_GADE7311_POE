using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();

    public int x;
    public static int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;
    public GameObject cardInDeck5;

    public GameObject cardToHand;
    public GameObject CardBack;
    public GameObject Deck;

    public GameObject[] Clones;

    public GameObject activeHand;
    public GameObject inactiveHand;


    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        deckSize = 50;

        //size of entire deck
        for (int i = 0; i < deckSize; i++)
        {
            //range of cards that can be chosen from
            x = Random.Range(0,5);
            deck[i] = CardDB.cardList[x];
        }

       StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {

        staticDeck = deck;
        if (deckSize < 40)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 30)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 20)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 10)
        {
            cardInDeck4.SetActive(false);
        }
        if (deckSize < 1)
        {
            cardInDeck5.SetActive(false);
        }
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i <= 6; i++)
        {
            yield return new WaitForSeconds(1);
            GameObject temp = Instantiate(cardToHand, activeHand.transform.position, transform.rotation, activeHand.transform);
            temp.GetComponent<CardToHand>().SendToActive();
        }

        for (int i = 0; i <= 6; i++)
        {
            yield return new WaitForSeconds(1);
            GameObject temp = Instantiate(cardToHand, inactiveHand.transform.position, transform.rotation, inactiveHand.transform);
            temp.GetComponent<CardToHand>().SendToInactive();
        }
    }
    
}
