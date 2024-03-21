using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCutoff : MonoBehaviour
{

    public AudioSource musicSource; // Reference to the AudioSource component

    public float fadeDuration = 5f; // Duration of the fade-out effect in seconds

    private float startVolume; // Initial volume of the audio source

    private float timer = 0f; // Timer to track the fade-out progress

    private bool fading = false; // Flag to check if fading is in progress

    void Start()
    {
        musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
        startVolume = musicSource.volume;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            StartFadeOut();
        }
    }

    void Update()
    {
        if (fading)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Calculate the new volume based on the elapsed time and fade duration
            float newVolume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);

            // Apply the new volume to the audio source
            musicSource.volume = newVolume;

            // Check if the fade-out is complete
            if (timer >= fadeDuration)
            {
                // Stop the music
                musicSource.Stop();
                // Reset the timer and fading flag
                timer = 0f;
                fading = false;
            }
        }
    }

    public void StartFadeOut()
    {
        // Start the fade-out process
        fading = true;
    }
}
