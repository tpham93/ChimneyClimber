using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text statsText;
    GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameStateObject = GameObject.Find("/GameState");
        gameState = gameStateObject.GetComponent<GameState>();
        statsText.text = GetStatsText();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private string GetStatsText()
    {
        GameScene[] levels = {
            GameScene.Level_1,
            GameScene.Level_2,
            GameScene.Level_3,
            GameScene.Level_4
        };

        LevelState sumLevelState = new LevelState(0, 0, 0, 0);

        foreach (GameScene level in levels)
        {
            LevelState levelState = gameState.GetLevelState(level);
            sumLevelState.numDeaths += levelState.numDeaths;
            sumLevelState.numCanes += levelState.numCanes;
            sumLevelState.numCollisionsWall += levelState.numCollisionsWall;
            sumLevelState.time += levelState.time;
        }

        string text = "";
        text += "Time: " + (sumLevelState.time).ToString("F1") + " s\n";
        text += "UsedCanes: " + (sumLevelState.numCanes) + "\n";
        text += "Wall collisions: " + (sumLevelState.numCollisionsWall) + "\n";
        text += "Deaths: " + (sumLevelState.numDeaths);
        return text;
    }

    public void OnClickContinue()
    {
        SceneLoader.LoadScene(GameScene.Main_Menu);
    }
}
