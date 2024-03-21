using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance { get; private set; }
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(music);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

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

    public void LevelComplete()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(endOfLevelWaitTime);
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
