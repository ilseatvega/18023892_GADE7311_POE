using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public void Start()
    {
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
        Application.Quit();
    }

    public void Forfeit()
    {

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
