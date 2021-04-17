using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToHand : MonoBehaviour
{
    public GameObject Hand;
    public GameObject theCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("Active_Hand");
        theCard.transform.SetParent(Hand.transform);
        theCard.transform.localScale = Vector3.one;
        theCard.transform.position = new Vector3(transform.position.x, transform.position.x, -48);
        theCard.transform.eulerAngles = new Vector3(25, 0, 0);
    }
}
