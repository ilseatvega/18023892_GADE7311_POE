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

    public TurnSystem ts;

    public GameObject activeHand;
    public GameObject inactiveHand;

    public int countIZ;
    public int countAZ;
    public int activeCounter;
    public int inactiveCounter;

    private RectTransform inactiveZone;
    private RectTransform activeZone;

    void Start()
    {
        //count = activeHand.transform.childCount;
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();
        inactiveZone = GameObject.FindGameObjectWithTag("IZ").GetComponent<RectTransform>();
        activeZone = GameObject.FindGameObjectWithTag("AZ").GetComponent<RectTransform>();

        //countIZ = inactiveZone.transform.childCount;
        //countAZ = activeZone.transform.childCount;
        inactiveCounter = -1;
        activeCounter = -1;

        x = 0;
        deckSize = 100;

        //size of entire deck
        for (int i = 0; i < deckSize; i++)
        {
            //range of cards that can be chosen from
            x = Random.Range(1,19);
            deck[i] = CardDB.cardList[x];
        }

       StartCoroutine(StartGame());
    }

    //
    void Update()
    {
        //setting cards that visually represent deck to false if the amount of cards i deck falls below certain amount
        staticDeck = deck;
        if (deckSize < 70)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 50)
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

        if (TurnSystem.startTurn == true)
        {
            //int count = activeHand.transform.childCount;
            //Debug.Log(count);
            if (ts.isPVP == true)
            {
                if (ts.count <= 6)
                {
                    //Debug.Log("im drawing");
                    StartCoroutine(Draw(1));
                    TurnSystem.startTurn = false;
                }
            }
            else
            {
                if (ts.isPlayer1Turn == true)
                {
                    if (ts.activeCount <= 6)
                    {
                        StartCoroutine(Draw(1));
                        TurnSystem.startTurn = false;
                    }
                }
                else
                {
                    if (ts.inactiveCount <= 6)
                    {
                        StartCoroutine(DrawAI(1));
                        TurnSystem.startTurn = false;
                    }
                }
            }
        }
    }

    //ienum to start the game by assigning both players cards to start with
    IEnumerator StartGame()
    {
        for (int i = 0; i <= 6; i++)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject temp = Instantiate(cardToHand, activeHand.transform.position, transform.rotation, activeHand.transform);
            temp.GetComponent<CardToHand>().SendToActive();
        }

        for (int i = 0; i <= 6; i++)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject temp = Instantiate(cardToHand, inactiveHand.transform.position, transform.rotation, inactiveHand.transform);
            temp.GetComponent<CardToHand>().SendToInactive();
        }
    }

    //ienum to draw x amount of cards
    IEnumerator Draw(int x)
    {
        countAZ = activeZone.transform.childCount;

        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject temp = Instantiate(cardToHand, activeHand.transform.position, transform.rotation, activeHand.transform);
            temp.GetComponent<CardToHand>().SendToActive();
        }

        //foreach (Transform child in this.transform)
        //{
        //    if (countAZ >= 7)
        //    {
        //        Debug.Log(activeCounter);
        //        Debug.Log(countAZ);
        //        this.transform.GetChild(activeCounter).gameObject.SetActive(false);
        //        //counter++;
        //    }
        //}
        //activeCounter++;
    }
    IEnumerator DrawAI(int x)
    {
        countIZ = inactiveZone.transform.childCount;
        countAZ = activeZone.transform.childCount;
        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject temp = Instantiate(cardToHand, inactiveHand.transform.position, transform.rotation, inactiveHand.transform);
            temp.GetComponent<CardToHand>().SendToInactive();
        }

        //foreach (Transform child in this.transform)
        //{
        //    if (countIZ >= 7)
        //    {
        //        Debug.Log(inactiveCounter);
        //        Debug.Log(countIZ);
        //        inactiveZone.transform.GetChild(inactiveCounter).gameObject.SetActive(false);
        //        //counter++;
        //    }
        //}
        //inactiveCounter++;
    }
}
