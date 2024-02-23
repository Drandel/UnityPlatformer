using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoController : MonoBehaviour
{
    public GameObject explosionEffect;
    bool isQuitting = false;
    private GameStateController gameState;
    public GameObject levelCompleteText;
    private LevelCompleteAnimation levelCompleteAnimation;
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        levelCompleteAnimation = levelCompleteText.GetComponent<LevelCompleteAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (!isQuitting && !PauseMenuController.IsPaused)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            levelCompleteAnimation.ShowLevelComplete();
            gameState.Level1Complete();
            // play fireworks
        }
    }
}
