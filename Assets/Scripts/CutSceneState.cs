
using UnityEngine;
using TMPro;
using System;
using System.Runtime.InteropServices;

public class CutSceneState : MonoBehaviour
{
    
    private int cutSceneStep = -1;
    
    public GameObject Boss;
    public GameObject Dino;
    public GameObject Platform1;
    public GameObject Platform2;
    public GameObject BossMusic;
    private bool first = true;
    private float waitTime = 3.0f;
    private float startTime;
    
    void Start()
    {
    }

    void Update()
    {
        switch (cutSceneStep)
        {
            case -1:
                break;
            case 0:
                if(first){
                    Dino.GetComponent<CharacterController>().BossCutScene();
                    first = false;
                    BossMusic.GetComponent<AudioSource>().Play();
                }else if(Dino.GetComponent<CharacterController>().walkDone){
                    first = true;
                    cutSceneStep = 1;
                }
                break;
            case 1:
                if(first){
                    Boss.GetComponent<Boss>().startCutSCene();
                    first = false;
                }else if(Boss.GetComponent<Boss>().atStartPos){
                    first = true;
                    cutSceneStep = 2;
                }
                break;
            case 2:
                if(first){
                    Dino.GetComponent<CharacterController>().lookUp();
                    startTime = Time.time;
                    first=false;
                }else if(Time.time - startTime >= waitTime){
                    Dino.GetComponent<CharacterController>().endCutScene();
                    Boss.GetComponent<Boss>().alert();
                    Platform1.GetComponent<MovingPlatform>().MovePlatform(); 
                    Platform2.GetComponent<MovingPlatform>().MovePlatform(); 
                    cutSceneStep = 3;
                }
                break;


        }
    }

    public void initCutScene(){
        cutSceneStep = 0;
    }

    
}