using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    private string currentLevel;
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // Subscribe ChangedActiveScene method to the activeSceneChanged event
        SceneManager.activeSceneChanged += ChangedActiveScene;
        currentLevel = "MainMenu";
    }

    void Update()
    {
        // if in level 1
        if(SceneManager.GetActiveScene().name == "Trex"){
            // Debug.Log("Level 1");
        }
    }

    // This code runs when scene is changed
    private void ChangedActiveScene(Scene current, Scene next)
    {
        Debug.Log($"Current {currentLevel}");
        Debug.Log($"Next {next.name}");
        // Debug.Log("Scenes: " + currentLevel + " changed to " + next.name);
        if(next.name == "MainMenu"){
            //dont change current level
        } else {
            currentLevel = next.name;
        }
    }
}
