using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuGroup;
    [SerializeField]
    GameObject creditsGroup;
    [SerializeField]
    GameObject GameStatePrefab;
    GameObject GameStateObject;
    // Start is called before the first frame update
    void Start()
    {
        GameStateObject = GameObject.Find("/GameState");
        if(GameStateObject == null)
        {
            GameStateObject = GameObject.Instantiate(GameStatePrefab);
            GameObject.DontDestroyOnLoad(GameStateObject);
            GameStateObject.name = "GameState";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMainMenuStart()
    {
        SceneLoader.LoadScene(GameScene.Tutorial);
    }

    public void OnClickMainMenuCredits()
    {
        mainMenuGroup.SetActive(false);
        creditsGroup.SetActive(true);
    }

    public void OnClickCreditsBack()
    {
        mainMenuGroup.SetActive(true);
        creditsGroup.SetActive(false);
    }
}
