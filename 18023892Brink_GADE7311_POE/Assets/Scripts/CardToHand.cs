using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToHand : MonoBehaviour
{
    public GameObject activeHand;
    public GameObject inactiveHand;
    public GameObject theCard;

    public Transform ActiveHand;
    public Transform InactiveHand;

    //finding the active and inactive hands and their transforms
    void Awake()
    {
        activeHand = GameObject.FindGameObjectWithTag("AH");
        inactiveHand = GameObject.FindGameObjectWithTag("IH");

        ActiveHand = GameObject.FindGameObjectWithTag("AH").transform;
        InactiveHand = GameObject.FindGameObjectWithTag("IH").transform;
    }
    //sending cards to activehand
    public void SendToActive()
    {
        theCard.transform.SetParent(activeHand.transform);
        theCard.transform.localScale = Vector3.one;
        theCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        theCard.transform.eulerAngles = new Vector3(25, 0, 0);
    }
    //sending cards to inactivehand
    public void SendToInactive()
    {
        theCard.transform.SetParent(inactiveHand.transform);
        theCard.transform.localScale = Vector3.one;
        theCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        theCard.transform.eulerAngles = new Vector3(25, 0, 0);
    }
    
}
