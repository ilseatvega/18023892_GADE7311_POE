using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToHand : MonoBehaviour
{
    public GameObject activeHand;
    public GameObject inactiveHand;
    public GameObject theCard;
    // Start is called before the first frame update
    void Awake()
    {
        activeHand = GameObject.FindGameObjectWithTag("AH");
        inactiveHand = GameObject.FindGameObjectWithTag("IH");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendToActive()
    {
        theCard.transform.SetParent(activeHand.transform);
        theCard.transform.localScale = Vector3.one;
        theCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        theCard.transform.eulerAngles = new Vector3(25, 0, 0);
    }

    public void SendToInactive()
    {
        theCard.transform.SetParent(inactiveHand.transform);
        theCard.transform.localScale = Vector3.one;
        theCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        theCard.transform.eulerAngles = new Vector3(25, 0, 0);
    }
}