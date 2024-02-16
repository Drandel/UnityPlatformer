using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUi;
    public string MainMenuScene;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(IsPaused){
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

    void Pause(){
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
