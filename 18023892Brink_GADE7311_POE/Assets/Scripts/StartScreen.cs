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
    string modePath;

    public Button basicAIButton;
    public Button exitAIOptions;
    public Canvas AIChoice;

    private setDifficulty aiDifficulty;
    public setDifficulty AI_Diff { get { return aiDifficulty; } }

    //List for Dropdown
    //List<string> dropOptions = new List<string> { "Magnus", "Nira", "Whiskey", "Zent" };

    public void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);

        //TEXT FILE PATH
        playerPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerNames.txt";
        //TEXT FILE PATH
        modePath = Application.dataPath + @"\ObjectData\TextFiles\GameMode.txt";

        //BUTTONS
        startButton.onClick.AddListener(StartGame);
        basicAIButton.onClick.AddListener(AIChoices);
        exitAIOptions.onClick.AddListener(CloseAIChoices);

        //DROPDOWN
        player1_DD.ClearOptions();
        //add options from list
        //player1_DD.AddOptions(dropOptions);
        //PLAYER 2 DD
        player2_DD.ClearOptions();
        //player2_DD.AddOptions(dropOptions);
    }

    public void StartGame()
    {
        HotseatGameMode();
        SaveTextInput();
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

    public void AIGameMode()
    {
        using (StreamWriter sw = new StreamWriter(modePath))
        {
            sw.WriteLine("AI");
        }
    }
    public void HotseatGameMode()
    {
        using (StreamWriter sw = new StreamWriter(modePath))
        {
            sw.WriteLine("hotseat");
        }
    }

    public void AIChoices()
    {
        AIChoice.gameObject.SetActive(true);
        //AIChoice.enabled = true;
    }

    public void CloseAIChoices()
    {
        AIChoice.gameObject.SetActive(false);
    }

    public void StartAIGame(int difficulty)
    {
        //conversion from int to enum
        aiDifficulty = (setDifficulty)difficulty;
        AIGameMode();
        SceneManager.LoadScene(1);
    }

    //}

    //public void SaveHero()
    //{
    //    using (StreamWriter sw = new StreamWriter(heroPath))
    //    {
    //        sw.WriteLine(player1_DD.value);
    //        sw.WriteLine(player2_DD.value);
    //    }
    //}
}
public enum setDifficulty
{
    easy = 0,
    normal = 1,
    hard = 2
}
