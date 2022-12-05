using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    GameObject goalGroup;
    [SerializeField]
    GameObject controlsGroup;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGoalBack()
    {
        SceneLoader.LoadScene(GameScene.Main_Menu);
    }

    public void OnGoalNext()
    {
        goalGroup.SetActive(false);
        controlsGroup.SetActive(true);
    }

    public void OnControlsStart()
    {
        SceneLoader.LoadScene(GameScene.Level_1);
    }

    public void OnControlsBack()
    {
        controlsGroup.SetActive(false);
        goalGroup.SetActive(true);
    }
}
