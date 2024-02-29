using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
// Static reference to the singleton instance
    public static MusicManager Instance { get; private set; }

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
    }

    // Add any other methods or variables related to sound management here
}
