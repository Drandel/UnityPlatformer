using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoController : MonoBehaviour
{
    public GameObject explosionEffect;
    bool isQuitting = false;
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }
}
