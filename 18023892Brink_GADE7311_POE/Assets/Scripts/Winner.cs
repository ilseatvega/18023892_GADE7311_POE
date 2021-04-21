using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Winner : MonoBehaviour
{
    public Canvas winCanvas;

    public Text p1storedName;
    public Text p2storedName;
    public Text p1WinName;
    public Text p2WinName;
    public Text drawText;

    public Button nextGame;
    public Button x;
    public Button exit;

    public PlayerDeck pd;
    public TurnSystem ts;

    public int totalHealth_P1;
    public int totalHealth_P2;

    void Start()
    {
        pd = GameObject.FindGameObjectWithTag("Deck").GetComponent<PlayerDeck>();
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();

        //hide canvas
    }
    
    void Update()
    {
        WhoWon();
    }

    public void NextGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        //delete files!!
        Application.Quit();
    }

    public void WhoWon()
    {
        totalHealth_P1 = ts.p1villageHealth + ts.p1militaryHealth;
        totalHealth_P2 = ts.p2villageHealth + ts.p2militaryHealth;

        //if deck runs out
        if (PlayerDeck.deckSize >= 0 )
        {
            if (totalHealth_P1 > totalHealth_P2)
            {
                Player1Won();
            }
            else if (totalHealth_P1 < totalHealth_P2)
            {
                Player2Won();
            }
            else if (totalHealth_P1 == totalHealth_P2)
            {
                Draw();
            }
        }

        if (ts.p1villageHealth == 0 && ts.p1militaryHealth == 0)
        {
            Player2Won();
        }
        else if (ts.p2villageHealth == 0 && ts.p2militaryHealth == 0)
        {
            Player1Won();
        }
    }

    public void Player1Won()
    {
        //set canvas to true
        //player1 txt true
    }

    public void Player2Won()
    {

    }

    public void Draw()
    {

    }
}
