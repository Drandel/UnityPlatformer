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
            levelCompleteText.SetActive(false);
        }
    }

    void Update()
    {
        // if in level 1
        if (SceneManager.GetActiveScene().name == "Trex")
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
        SceneManager.LoadScene($"Level{currentLevel + 1}");
        currentLevel += 1;
    }
}
