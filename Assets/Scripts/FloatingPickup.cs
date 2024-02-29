using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    public float floatRange = 1f; // Range of floating motion
    public float floatSpeed = 1f; // Speed of floating motion
    private GameStateController gameState;
    public AudioClip sound;
    public AudioSource soundSource;
    private bool hasHit = false;
    private BoxCollider2D coll;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Start floating up and down
        coll = gameObject.GetComponent<BoxCollider2D>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Float();
    }

    private void Float()
    {
        // Calculate the target positions
        float startY = transform.position.y;
        float targetY = startY + floatRange;

        // Use LeanTween to animate the floating motion
        LeanTween.moveY(gameObject, targetY, floatSpeed)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                // When reaching the top, float down
                FloatDown();
            });
    }

    private void FloatDown()
    {
        // Calculate the target positions
        float startY = transform.position.y;
        float targetY = startY - floatRange;

        // Use LeanTween to animate the floating motion
        LeanTween.moveY(gameObject, targetY, floatSpeed)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                // When reaching the bottom, float up again
                Float();
            });
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !hasHit)
        {
            soundSource.PlayOneShot(sound);
            gameState.lifeCount += 1;
            hasHit = true;
            coll.enabled = false; // Disable the collider
            spriteRenderer.enabled = false; // Disable the sprite renderer
            // Optionally, you can also add particle effects, visual effects, or other actions here
            
            // Delayed destruction
            Destroy(gameObject, sound.length); // Destroy the GameObject after the sound finishes playing
        }
    }
}
