using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Winner : MonoBehaviour
{
    public Canvas winCanvas;

    //name in txt file - text box from in game
    public Text p1storedName;
    public Text p2storedName;
    public string p1Name;
    public string p2Name;
    //actual text displayed in this canvas
    public Text p1WinName;
    public Text p2WinName;
    public Text drawText;
    public Text commonText;

    public Button nextGame;
    public Button x;
    public Button exit;

    public PlayerDeck pd;
    public TurnSystem ts;

    public int totalHealth_P1;
    public int totalHealth_P2;

    string playerPath;
    string modePath;

    void Start()
    {
        playerPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerNames.txt";
        modePath = Application.dataPath + @"\ObjectData\TextFiles\GameMode.txt";

        pd = GameObject.FindGameObjectWithTag("Deck").GetComponent<PlayerDeck>();
        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();

        winCanvas = GameObject.Find("WinCanvas").GetComponent<Canvas>();

        nextGame.onClick.AddListener(NextGame);
        x.onClick.AddListener(Exit);
        exit.onClick.AddListener(Exit);

        winCanvas.enabled = false;
    }
    
    void Update()
    {
        WhoWon();
    }

    public void NextGame()
    {
        File.Delete(playerPath);
        File.Delete(modePath);

        //deleting meta files as well - not sure if necessary but did it anyway
        File.Delete(playerPath + ".meta");
        File.Delete(modePath + ".meta");

        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        File.Delete(playerPath);
        File.Delete(modePath);

        //deleting meta files as well - not sure if necessary but did it anyway
        File.Delete(playerPath + ".meta");
        File.Delete(modePath + ".meta");

        Application.Quit();
    }

    public void WhoWon()
    {
        totalHealth_P1 = ts.p1villageHealth + ts.p1militaryHealth;
        totalHealth_P2 = ts.p2villageHealth + ts.p2militaryHealth;
        //Debug.Log(PlayerDeck.deckSize);

        //if deck runs out
        if (PlayerDeck.deckSize <= 0)
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

        //if one player loses both populations
        if (ts.p1villageHealth <= 0 && ts.p1militaryHealth <= 0)
        {
            Player2Won();
        }
        else if (ts.p2villageHealth <= 0 && ts.p2militaryHealth <= 0)
        {
            Player1Won();
        }
    }

    public void Player1Won()
    {
        winCanvas.enabled = true;
        p1WinName.enabled = true;
        p2WinName.enabled = false;
        p1Name = p1storedName.text;
        p1WinName.text = "congratulations, " + p1Name + ".";
    }

    public void Player2Won()
    {
        winCanvas.enabled = true;
        p1WinName.gameObject.SetActive(false);
        p2WinName.gameObject.SetActive(true);
        p2Name = p2storedName.text;
        p2WinName.text = "congratulations, " + p2Name + ".";
    }

    public void Draw()
    {
        winCanvas.enabled = true;
        p1WinName.enabled = false;
        p2WinName.enabled = false;
        commonText.enabled = false;
        drawText.enabled = true;
    }
}
