using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackPrefab : MonoBehaviour
{
    public GameObject Deck;
    public GameObject theCard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Deck = GameObject.Find("DeckPanel");
        theCard.transform.SetParent(Deck.transform);
        theCard.transform.localScale = Vector3.one;
        theCard.transform.position = new Vector3(transform.position.x, transform.position.x, -48);
        theCard.transform.eulerAngles = new Vector3(25, 0, 0);
    }
}
