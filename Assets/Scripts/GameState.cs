using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState
{
    public int numDeaths;
    public int numCanes;
    public int numCollisionsWall;
    public float time;

    public LevelState(int numDeaths, int numCanes, int numCollisionsWall, int time)
    {
        this.numDeaths = numDeaths;
        this.numCanes = numCanes;
        this.numCollisionsWall = numCollisionsWall;
        this.time = time;
    }
}

[Serializable]
public class ServerData
{
    public string remoteServerAddress = null;
    public string localRedeemAddress = null;
    public string secret = null;
    public string authToken = null;
}

public class GameState : MonoBehaviour
{
    [SerializeField]
    GameScene lastScene = GameScene.Main_Menu;

    public GameScene LastScene{
        get{
            return lastScene;
        }
        set{
            lastScene = value;
        }
    }

    [SerializeField]
    GameScene currentScene = GameScene.Main_Menu;
    public GameScene CurrentScene{
        get{
            return currentScene;
        }
        set{
            currentScene = value;
        }
    }

    ServerData serverData;
    public ServerData CurrentServerData{
        get{
            return serverData;
        }
    }

    private Dictionary<GameScene, LevelState> levelStates;

    void Start()
    {
        levelStates = new Dictionary<GameScene, LevelState>();

        TextAsset serverDataTextAsset = Resources.Load<TextAsset>("Hidden/ServerData");
        if(serverDataTextAsset != null)
        {
            serverData = JsonUtility.FromJson<ServerData>(serverDataTextAsset.text);
        }
        else
        {
            serverData = new ServerData();
        }
    }

    public void AddLevelState(GameScene gameScene, LevelState levelState)
    {
        levelStates[gameScene] = levelState;
    }

    public LevelState GetLevelState(GameScene gameScene)
    {
        return levelStates[gameScene];
    }
}
