using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    private int currentLevel = 1;
    public int lifeCount = 3;
    public GameObject music;
    public GameObject levelCompleteText;
    public float endOfLevelWaitTime;
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(music);
    }
    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        levelCompleteText = GameObject.Find("LevelCompleteText");

        if(scene.name != "MainMenu"){
            levelCompleteText.SetActive(false);
        }
    }
    void Start()
    {
        // Subscribe ChangedActiveScene method to the activeSceneChanged event
        // SceneManager.activeSceneChanged += ChangedActiveScene;
        // currentLevel = "MainMenu";
    }

    void Update()
    {
        // if in level 1
        if(SceneManager.GetActiveScene().name == "Trex"){
            // Debug.Log("Level 1");
        }
    }

    public void LevelComplete()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(endOfLevelWaitTime);

        // Load the next scene
        Debug.Log(currentLevel);
        SceneManager.LoadScene($"Level{currentLevel+1}");
        currentLevel += 1;
    }
}
