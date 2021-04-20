using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public bool isPlayer1Turn;
    //public static bool isPlayer1Turn;
    public int p1Turn;
    public int p2Turn;
    public RawImage player1;
    public RawImage player2;
    public Text p1Name;
    public Text p2Name;

    public int p1maxMana;
    public int p1currentMana;
    //public static int p1currentMana;
    public int p2maxMana;
    public int p2currentMana;
    //public static int p2currentMana;

    public int p1villageHealth;
    public int p1militaryHealth;
    public int p2villageHealth;
    public int p2militaryHealth;

    public Text p1villageText;
    public Text p2villageText;
    public Text p1militaryText;
    public Text p2militaryText;

    public Text p1manaText;
    public Text p2manaText;

    private RectTransform inactiveHand;
    private RectTransform activeHand;

    // Start is called before the first frame update
    void Start()
    {
        isPlayer1Turn = true;
        p1Turn = 1;
        p2Turn = 1;

        p1maxMana = 1;
        p1currentMana = 1;

        p2maxMana = 1;
        p2currentMana = 1;

        p1manaText.text = "1";
        p2manaText.text = "1";

        p1villageHealth = 350;
        p2villageHealth = 350;
        //p1villageText.text = p1villageHealth.ToString();
        //p2villageText.text = p2villageHealth.ToString();

        p1militaryHealth = 350;
        p2militaryHealth = 350;
        //p1militaryText.text = p1villageHealth.ToString();
        //p2militaryText.text = p2villageHealth.ToString();

        inactiveHand = GameObject.FindGameObjectWithTag("IH").GetComponent<RectTransform>();
        activeHand = GameObject.FindGameObjectWithTag("AH").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if player turn enable all player 1 ui
        if (isPlayer1Turn == true)
        {
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(false);
            p1Name.gameObject.SetActive(true);
            p2Name.gameObject.SetActive(false);
        }
        //if player turn enable all player 2 ui
        else if (isPlayer1Turn == false)
        {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(true);
            p1Name.gameObject.SetActive(false);
            p2Name.gameObject.SetActive(true);
        }
    }

    //press pass button to end turn
    public void EndTurn()
    {
        //PLAYER 1 SWITCHES TO INACTIVE (TOP)
        if (isPlayer1Turn == true)
        {
            p2Turn += 1;
            if (p2maxMana != 10)
            {
                p2maxMana += 1;
            }
            p2currentMana = p2maxMana;

            //p1manaText.text = p2currentMana.ToString();
            //p2manaText.text = p1currentMana.ToString();
            p1manaText.text = p2currentMana + "/" + p2maxMana;
            p2manaText.text = p1currentMana + "/" + p1maxMana;
            
            isPlayer1Turn = false;
        }
        //PLAYER 1 SWITCHES TO ACTIVE (BOTTOM)
        else if (isPlayer1Turn == false)
        {
            p1Turn += 1;
            if (p1maxMana != 10)
            {
                p1maxMana += 1;
            }
            p1currentMana = p1maxMana;

            //p1currentMana = p1maxMana;
            //p2manaText.text = p2currentMana.ToString();
            p1manaText.text = p1currentMana + "/" + p1maxMana;
            p2manaText.text = p2currentMana + "/" + p2maxMana;

            isPlayer1Turn = true;
        }

        //card switching
        SwitchPlayerCards();

        for (int i = 0; i < inactiveHand.childCount; i++)
        {
            if (inactiveHand.GetChild(i))
            {
                if (inactiveHand.GetChild(i).GetComponent<Draggable>())
                {
                    inactiveHand.GetChild(i).GetComponent<Draggable>().Disable();
                }
            }
        }

        for (int i = 0; i < activeHand.childCount; i++)
        {
            if (activeHand.GetChild(i))
            {
                if (activeHand.GetChild(i).GetComponent<Draggable>())
                {
                    activeHand.GetChild(i).GetComponent<Draggable>().Enable();
                }
            }
        }

    }

    public void RemoveMana(byte playerID, int amount)
    {
        if (playerID == 1)
        {
            p1currentMana -= amount;
            p1currentMana = Mathf.Clamp(p1currentMana, 0, 10);
        }
        else if (playerID == 2)
        {
            p2currentMana -= amount;
            p2currentMana = Mathf.Clamp(p1currentMana, 0, 10);
        }
    }

    public void SwitchPlayerCards()
    {
        int count = activeHand.childCount;
        int count2 = inactiveHand.childCount;

        //activehand children to inactive hand
        for (int i = 0; i < count; i++)
        {
              activeHand.GetChild(0).SetParent(inactiveHand);
        }
        //inactivehand children to active hand
        for (int i = 0; i < inactiveHand.childCount; i++)
        {
            inactiveHand.GetChild(0).SetParent(activeHand);
        }
    }
}