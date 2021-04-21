using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class DeleteTest : MonoBehaviour
{
    string playerPath;
    //string heroPath;
    public Button quitButton;

    public void Start()
    {
        playerPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerNames.txt";
        //heroPath = Application.dataPath + @"\ObjectData\TextFiles\PlayerHeroes.txt";
    }

    public void Update()
    {
        quitButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        File.Delete(playerPath);
        //File.Delete(heroPath);
        //deleting meta files as well - not sure if necessary but did it anyway
        File.Delete(playerPath + ".meta");
        //File.Delete(heroPath + ".meta");
    }
}
