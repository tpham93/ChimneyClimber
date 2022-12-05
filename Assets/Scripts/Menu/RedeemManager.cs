using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RedeemManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text title;
    [SerializeField]
    TMP_Text statsText;
    [SerializeField]
    TMP_Text redeemText;
    GameState gameState;
    int finishedLevel;
    private bool registeredFinished;
    private bool registeredSuccesfully;
    private bool enableRedeem;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameStateObject = GameObject.Find("/GameState");
        gameState = gameStateObject.GetComponent<GameState>();
        string lastLevelString = gameState.LastScene.ToString();
        string levelStr = lastLevelString.Split('_')[1];
        title.text = "Finished level " + levelStr + "!";
        enableRedeem = gameState.CurrentServerData.localRedeemAddress == null;
        if(!enableRedeem)
        {
            redeemText.enabled=false;
        }
        registeredSuccesfully = false;
        finishedLevel = int.Parse(levelStr);
        RegisterHighscore();
        statsText.text = GetStatsText();
    }

    // Update is called once per frame
    void Update()
    {
        if(enableRedeem)
        {
            string redeemSuccesfullText = "Pending";
            if(registeredFinished)
            {
                if(registeredSuccesfully)
                {
                    redeemSuccesfullText = "Succesfull";
                }
                else
                {
                    redeemSuccesfullText = "Unsuccesfull";
                }
            }
            redeemText.text = "Redeem:\n" + redeemSuccesfullText;
        }
    }

    private string GetStatsText()
    {
        LevelState levelState = gameState.GetLevelState(gameState.LastScene);
        string text = "";
        text += "Time: " + (levelState.time).ToString("F1") + " s\n";
        text += "Used canes: " + (levelState.numCanes) + "\n";
        text += "Wall collisions: " + (levelState.numCollisionsWall) + "\n";
        text += "Deaths: " + (levelState.numDeaths);
        return text;
    }

    public void OnClickContinue()
    {
        SceneLoader.LoadScene(GetNextScene(gameState.LastScene));
    }

    GameScene GetNextScene(GameScene lastScene)
    {
        switch (lastScene)
        {
            case GameScene.Level_1:
                return GameScene.Level_2;
            case GameScene.Level_2:
                return GameScene.Level_3;
            case GameScene.Level_3:
                return GameScene.Level_4;
            case GameScene.Level_4:
                return GameScene.Game_Over;
            default:
                return GameScene.Main_Menu;
        }
    }

    void RegisterHighscore()
    {
        RedeemPoints(finishedLevel*50);
    }

    private void RedeemPoints(int points)
    {
        registeredFinished = false;
        ServerData serverData = gameState.CurrentServerData;
        if(serverData.localRedeemAddress != null)
        {
#if UNITY_EDITOR
            string base_uri = serverData.remoteServerAddress + serverData.localRedeemAddress;
            string uri = base_uri+"?secret="+serverData.secret+"&points="+points.ToString()+"&game_token="+serverData.authToken;
#else
            string base_uri = serverData.localRedeemAddress;
            string uri = base_uri+"?secret="+serverData.secret+"&points="+points.ToString();
#endif
            StartCoroutine(SendRequest(uri));
        }
    }

    private IEnumerator SendRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            registeredFinished = true;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.Log( "Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.Log(  "HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(  "Received: " + webRequest.downloadHandler.text);
                        registeredSuccesfully = true;
                        break;
            }
        }
    }

}
