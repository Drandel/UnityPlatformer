using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUi;
    public string MainMenuScene;
    public bool isGameOver = false;
    public Button resumeButton;
    public Button gameOverButton;

    private void Awake() {
        // DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(IsPaused && !isGameOver){
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume(){
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Pause(bool gameOver = false){
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        isGameOver = gameOver;
        IsPaused = true;
        if(isGameOver){
            resumeButton.gameObject.SetActive(false);
            gameOverButton.gameObject.SetActive(true);
        }
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
