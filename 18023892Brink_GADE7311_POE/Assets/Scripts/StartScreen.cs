using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class StartScreen : MonoBehaviour
{
    public InputField player1Name;
    public InputField player2Name;
    public Button startButton;
    public Dropdown player1_DD;
    public Dropdown player2_DD;
    string playerPath;
    string heroPath;

    //List for Dropdown
    List<string> dropOptions = new List<string> { "Magnus", "Nira", "Whiskey", "Zent" };

    public void Start()
    {
        //TEXT FILE PATH
        playerPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerNames.txt";
        //TEXT FILE PATH
        heroPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerHeroes.txt";

        //START BUTTON
        startButton.onClick.AddListener(StartGame);

        //DROPDOWN
        player1_DD.ClearOptions();
        //add options from list
        player1_DD.AddOptions(dropOptions);
        //PLAYER 2 DD
        player2_DD.ClearOptions();
        player2_DD.AddOptions(dropOptions);
    }

    public void StartGame()
    {
        SaveTextInput();
        SaveHero();
        SceneManager.LoadScene(1);
    }

    public void SaveTextInput()
    {
        using (StreamWriter sw = new StreamWriter(playerPath))
        {
            sw.WriteLine(player1Name.text);
            sw.WriteLine(player2Name.text);
        }
    }

    public void SaveHero()
    {
        using (StreamWriter sw = new StreamWriter(heroPath))
        {
            sw.WriteLine(player1_DD.value);
            sw.WriteLine(player2_DD.value);
        }
    }
}
