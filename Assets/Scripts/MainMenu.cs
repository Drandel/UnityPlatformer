using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string mainScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitButton(){
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void StartButton(){
        SceneManager.LoadScene(mainScene);
    }
}
