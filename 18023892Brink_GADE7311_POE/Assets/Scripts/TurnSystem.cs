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

    public Text p1manaText;
    public Text p2manaText;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1Turn == true)
        {
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(false);
            p1Name.gameObject.SetActive(true);
            p2Name.gameObject.SetActive(false);
        }
        else if (isPlayer1Turn == false)
        {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(true);
            p1Name.gameObject.SetActive(false);
            p2Name.gameObject.SetActive(true);
        }
    }

    public void EndTurn()
    {
        //PLAYER 1 SWITCHES TO INACTIVE (TOP)
        if (isPlayer1Turn == true)
        {
            p2Turn += 1;
            p2maxMana += 1;
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
            p1maxMana += 1;
            p1currentMana = p1maxMana;

            //p1currentMana = p1maxMana;
            //p2manaText.text = p2currentMana.ToString();
            p1manaText.text = p1currentMana + "/" + p1maxMana;
            p2manaText.text = p2currentMana + "/" + p2maxMana;

            isPlayer1Turn = true;
        }
    }
}
