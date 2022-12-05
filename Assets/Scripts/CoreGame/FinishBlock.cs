using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinishBlock : MonoBehaviour
{
    PresentManager presentManager;
    // Start is called before the first frame update
    void Start()
    {
        presentManager = GameObject.Find("/PresentManager").GetComponent<PresentManager>();
#if UNITY_EDITOR
        GetComponent<PlayerInput>().enabled = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(presentManager.CollectedAllPresents())
        {
            TriggerEnd();
        }
    }

    void TriggerEnd()
    {
        GameState gameState = GameObject.Find("/GameState").GetComponent<GameState>();
        Player player = GameObject.Find("/Player").GetComponent<Player>();
        gameState.AddLevelState(gameState.CurrentScene, player.CurrentLevelState);
        SceneLoader.LoadScene(GameScene.Redeem);
    }

    public void OnCheat(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        if(context.started)
        {
            TriggerEnd();
        }
#endif
    }
}
