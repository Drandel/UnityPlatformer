using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    // Static reference to the singleton instance
    public static GameStateController Instance { get; private set; }

    // Your game state variables
    private int currentLevel = 1;
    public int lifeCount = 3;
    public GameObject music;
    public GameObject levelCompleteText;
    public float endOfLevelWaitTime;
    public int enemiesKilled = 0;
    private float startTime;
    private float endTime;

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy it
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this object if it doesn't exist
        Instance = this;

        // Don't destroy this object when loading new scenes
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(music);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelCompleteText = GameObject.Find("LevelCompleteText");

        if (scene.name != "MainMenu")
        {
            if(scene.name == "Level 1"){
                startTime = Time.time;
            }
            levelCompleteText.SetActive(false);
        } else {
            lifeCount = 3;
            currentLevel = 1;
        }
    }

    void Update()
    {
        // if in level 1
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            // Debug.Log("Level 1");
        }
    }

    public void LevelComplete()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(endOfLevelWaitTime);
        // Wait for the specified amount of time

        // Load the next scene
        if(currentLevel < 3){
            SceneManager.LoadScene($"Level{currentLevel + 1}");
        } else {
            endTime = Time.time;
            SceneManager.LoadScene("Scoreboard");
        }
        
        currentLevel += 1;
    }

    public float getTimeSpent()
    {
        return (float)Math.Round(endTime-startTime, 2);
    }
}
