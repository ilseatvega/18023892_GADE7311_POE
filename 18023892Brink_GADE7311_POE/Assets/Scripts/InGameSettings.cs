using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class InGameSettings : MonoBehaviour
{
    public Canvas settingsCanvas;
    public Canvas ruleCanvas;

    public Button settings;
    public Button restart;
    public Button rulebook;
    public Button exitRulebook;
    public Button forfeit;
    public Button quit;
    public Button exitSettings;

    public TurnSystem ts;

    //HERO CARDS ARE A WORK IN PROGRESS (WIP) AND MIGHT NOT BE ADDED TO THE FINAL GAME
    //string heroPath;

    string playerPath;
    
    public void Start()
    {
        playerPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerNames.txt";
        //heroPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerHeroes.txt";

        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();

        settings.onClick.AddListener(Settings);
        restart.onClick.AddListener(Restart);
        rulebook.onClick.AddListener(Rules);
        exitRulebook.onClick.AddListener(ExitRules);
        forfeit.onClick.AddListener(Forfeit);
        quit.onClick.AddListener(QuitGame);
        exitSettings.onClick.AddListener(ExitSettings);
    }

    public void Settings()
    {
        settingsCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        //SceneManager.LoadScene("Main");
    }

    public void Rules()
    {
        ruleCanvas.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        File.Delete(playerPath);
        //File.Delete(heroPath);

        //deleting meta files as well - not sure if necessary but did it anyway
        File.Delete(playerPath + ".meta");
        //File.Delete(heroPath + ".meta");
        Application.Quit();
    }

    public void Forfeit()
    {
        if (ts.isPlayer1Turn == true)
        {
            //player2 wins
        }
        else
        {
            //player 1 wins
        }
    }

    public void ExitSettings()
    {
        settingsCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitRules()
    {
        ruleCanvas.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
}
