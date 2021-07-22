using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class TurnSystem : MonoBehaviour
{
    public DefendState defend;
    public StateManager sm;
    public AI_Player ai;

    public bool isPlayer1Turn;
    public bool isPVP = true;
    public bool mode = false;
    public string gameMode;
    public string modePath;

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

    public RectTransform inactiveHand;
    public RectTransform activeHand;

    public RectTransform inactiveZone;
    public RectTransform activeZone;

    public int damageHolder = 0;
    public bool isAttacking = false;
    public bool villageAttack = false;
    public bool militaryAttack = false;

    public Button village;
    public Button military;

    public static bool startTurn;

    public Button pass;
    //public static bool startTurn_2;

    public int count;
    public int activeCount;
    public int inactiveCount;

    public Image passImage;
    public Text passText;

    public GameObject states;

    //public GameObject emptyGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        pass.enabled = false;
        passImage = pass.gameObject.GetComponent<Image>();
        passImage.enabled = false;
        passText.enabled = false;

        StartCoroutine(PassActivate());

        isPlayer1Turn = true;
        p1Turn = 1;
        p2Turn = 1;

        p1maxMana = 1;
        p1currentMana = 1;

        p2maxMana = 1;
        p2currentMana = 1;

        p1manaText.text = "1";
        p2manaText.text = "1";

        p1villageHealth = 100;
        p2villageHealth = 100;

        p1militaryHealth = 100;
        p2militaryHealth = 100;

        modePath = Application.dataPath + @"\ObjectData\TextFiles\GameMode.txt";
        GameMode();

        sm = GameObject.FindGameObjectWithTag("IH").GetComponent<StateManager>();
        ai = GameObject.FindGameObjectWithTag("AI").GetComponent<AI_Player>();

        inactiveHand = GameObject.FindGameObjectWithTag("IH").GetComponent<RectTransform>();
        activeHand = GameObject.FindGameObjectWithTag("AH").GetComponent<RectTransform>();

        inactiveZone = GameObject.FindGameObjectWithTag("IZ").GetComponent<RectTransform>();
        activeZone = GameObject.FindGameObjectWithTag("AZ").GetComponent<RectTransform>();

        village = GameObject.FindGameObjectWithTag("VB").GetComponent<Button>();
        military = GameObject.FindGameObjectWithTag("MB").GetComponent<Button>();

        village.onClick.AddListener(GameObject.Find("CardBG").GetComponent<ThisCard>().VillageAttack);
        military.onClick.AddListener(GameObject.Find("CardBG").GetComponent<ThisCard>().MilitaryAttack);

        GameMode();
        if (mode == true)
        {
            Destroy(states);
        }
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

    public void GameMode()
    {
        using (StreamReader sw = new StreamReader(modePath))
        {
            gameMode = File.ReadLines(modePath).ElementAt(0);
            if (gameMode == "AI")
            {
                isPVP = false;
            }
            else if (gameMode == "advanced")
            {
                mode = true;
            }
            else
            {
                isPVP = true;
            }
        }
    }
    //press pass button to end turn
    public void EndPvPTurn()
    {
            //PLAYER 1 SWITCHES TO INACTIVE (TOP)
            if (isPlayer1Turn == true)
            {
                count = inactiveHand.transform.childCount;
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

                startTurn = true;
                isPlayer1Turn = false;
            }
            //PLAYER 1 SWITCHES TO ACTIVE (BOTTOM)
            else if (isPlayer1Turn == false)
            {
                count = inactiveHand.transform.childCount;

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

                startTurn = true;
                isPlayer1Turn = true;
            }

            //card switching
            SwitchPlayerCards();
            //zone switching
            SwitchPlayerZones();

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

    public void EndAITurn()
    {
        count = activeHand.transform.childCount;
        sm.currentState = defend;
        //AI turn
        if (isPlayer1Turn == true)
        {
            activeCount = activeHand.transform.childCount;
            pass.enabled = false;
            
            //disable player dragging cards
            for (int i = 0; i < count; i++)
            {
                if (activeHand.GetChild(i))
                {
                    if (activeHand.GetChild(i).GetComponent<Draggable>())
                    {
                        activeHand.GetChild(i).GetComponent<Draggable>().Disable();
                    }
                }
            }

            p2Turn += 1;
            if (p2maxMana != 10)
            {
                p2maxMana += 1;
            }
            p2currentMana = p2maxMana;
            
            p2manaText.text = p2currentMana + "/" + p2maxMana;
            p1manaText.text = p1currentMana + "/" + p1maxMana;

            startTurn = true;
            isPlayer1Turn = false;
            if (mode == true)
            {
                if (isPlayer1Turn == false)
                {
                    ai.PlayHighestCard();
                }
            }
        }
        //PLAYER turn
        else if (isPlayer1Turn == false)
        {
            PassTurnToPlayer();
        }
    }

    public void EndTurn()
    {
        if (isPVP == true)
        {
            EndPvPTurn();
        }
        else
        {
            CheckHealthBelowZero();
            EndAITurn();
        }
    }

    public void PassTurnToPlayer()
    {
        inactiveCount = inactiveHand.transform.childCount;
        pass.enabled = true;
        
        //enable player dragging cards
        for (int i = 0; i < count; i++)
        {
            if (activeHand.GetChild(i))
            {
                if (activeHand.GetChild(i).GetComponent<Draggable>())
                {
                    activeHand.GetChild(i).GetComponent<Draggable>().Enable();
                }
            }
        }

        p1Turn += 1;
        if (p1maxMana != 10)
        {
            p1maxMana += 1;
        }
        p1currentMana = p1maxMana;

        p1manaText.text = p1currentMana + "/" + p1maxMana;
        p2manaText.text = p2currentMana + "/" + p2maxMana;
        
        startTurn = true;
        isPlayer1Turn = true;
    }

    public void CheckHealthBelowZero()
    {
        if (p1militaryHealth <= 0)
        {
            p1militaryHealth = 0;
        }
        else if (p2militaryHealth <= 0)
        {
            p2militaryHealth = 0;
        }
        else if (p1villageHealth <= 0)
        {
            p1villageHealth = 0;
        }
        else if (p2villageHealth <= 0)
        {
            p2villageHealth = 0;
        }

        p1militaryText.text = p1militaryHealth.ToString();
        p2militaryText.text = p2militaryHealth.ToString();
        p1villageText.text = p1villageHealth.ToString();
        p2villageText.text = p2villageHealth.ToString();
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
            p2currentMana = Mathf.Clamp(p2currentMana, 0, 10);
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
        for (int i = 0; i < count2; i++)
        {
            inactiveHand.GetChild(0).SetParent(activeHand);
        }
    }

    public void SwitchPlayerZones()
    {
        int count = activeZone.childCount;
        int count2 = inactiveZone.childCount;

        //activezone children to inactive zone
        for (int i = 0; i < count; i++)
        {
            activeZone.GetChild(0).SetParent(inactiveZone);
        }
        //inactivezone children to active zone
        for (int i = 0; i < count2; i++)
        {
            inactiveZone.GetChild(0).SetParent(activeZone);
        }
    }

    IEnumerator PassActivate()
    {
        yield return new WaitForSeconds(4.6f);
        pass.enabled = true;
        passImage.enabled = true;
        passText.enabled = true;
    }
}