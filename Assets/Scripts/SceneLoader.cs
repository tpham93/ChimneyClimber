using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScene
{
    Main_Menu,
    Tutorial,
    Redeem,
    Game_Over,
    Level_1,
    Level_2,
    Level_3,
    Level_4
}

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(GameScene scene)
    {
        GameObject gameStateObject = GameObject.Find("/GameState");
        GameState gameState = gameStateObject.GetComponent<GameState>();

        gameState.LastScene = gameState.CurrentScene;
        gameState.CurrentScene = scene;
        AsyncOperation progress = SceneManager.LoadSceneAsync(
            scene.ToString(),
            LoadSceneMode.Single
        );
    }
}
