using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class LoadPlayerInfo : MonoBehaviour
{
    string playerPath;
    //string heroPath;

    public Text turnName_P1;
    public Text turnName_P2;

    public Text popName_P1;
    public Text popName_P2;

    string player1;
    string player2;


    void Start()
    {
        playerPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerNames.txt";
        //heroPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerHeroes.txt";

        LoadTurnNames();
    }

    public void LoadTurnNames()
    {
        using (StreamReader sw = new StreamReader(playerPath))
        {
            player1 = File.ReadLines(playerPath).ElementAt(0);
            player2 = File.ReadLines(playerPath).ElementAt(1);
        }
        turnName_P1.text = player1;
        turnName_P2.text = player2;
        popName_P1.text = player1;
        popName_P2.text = player2;
    }
}
