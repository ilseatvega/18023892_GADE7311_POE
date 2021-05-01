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
    public Canvas boardLayout;

    public Button settings;
    public Button rulebook;
    public Button exitRulebook;
    public Button forfeit;
    public Button quit;
    public Button exitSettings;
    public Button backToRules;
    public Button toBoardLayout;

    public TurnSystem ts;
    public Winner winner;
    
    string playerPath;
    
    public void Start()
    {
        playerPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerNames.txt";

        ts = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnSystem>();
        winner = GameObject.FindGameObjectWithTag("Manager").GetComponent<Winner>();

        settings.onClick.AddListener(Settings);
        rulebook.onClick.AddListener(Rules);
        exitRulebook.onClick.AddListener(ExitRules);
        forfeit.onClick.AddListener(Forfeit);
        quit.onClick.AddListener(QuitGame);
        exitSettings.onClick.AddListener(ExitSettings);
        backToRules.onClick.AddListener(BackToRules);
        toBoardLayout.onClick.AddListener(Layout);

    }

    public void Settings()
    {
        settingsCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Rules()
    {
        ruleCanvas.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        File.Delete(playerPath);
        //deleting meta files as well - not sure if necessary but did it anyway
        File.Delete(playerPath + ".meta");

        Application.Quit();
    }

    public void Forfeit()
    {
        if (ts.isPlayer1Turn == true)
        {
            winner.Player2Won();
        }
        else
        {
            winner.Player1Won();
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

    public void BackToRules()
    {
        boardLayout.gameObject.SetActive(false);
        ruleCanvas.gameObject.SetActive(true);
    }

    public void Layout()
    {
        boardLayout.gameObject.SetActive(true);
        ruleCanvas.gameObject.SetActive(false);
    }
}
