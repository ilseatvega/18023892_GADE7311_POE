using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public bool isPlayer1Turn;
    public int p1Turn;
    public int p2Turn;
    public Image player1;
    public Image player2;

    public int p1maxMana;
    public int p1currentMana;
    public int p2maxMana;
    public int p2currentMana;
    public Text manaText;

    // Start is called before the first frame update
    void Start()
    {
        isPlayer1Turn = true;
        p1Turn = 1;
        p2Turn = 0;

        p1maxMana = 1;
        p1currentMana = 1;

        p2maxMana = 1;
        p2currentMana = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1Turn == true)
        {
            player1.enabled = true;
            player2.enabled = false;
        }
        else
        {
            player1.enabled = false;
            player2.enabled = true;
        }
    }
}
