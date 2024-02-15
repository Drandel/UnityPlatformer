using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{   
    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip alienHitSound;
    public AudioSource audioSource;
    private bool hasHit = false;
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer spriteRenderer;
    public float damage = 10f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shootSound);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!hasHit && other.gameObject.CompareTag("Ground"))
        {
            // Play the hit sound effect
            audioSource.PlayOneShot(hitSound);
            hasHit = true;
            rb.velocity = Vector2.zero; // Stop the bullet's movement
            coll.enabled = false; // Disable the collider
            spriteRenderer.enabled = false; // Disable the sprite renderer
            // Optionally, you can also add particle effects, visual effects, or other actions here
            
            // Delayed destruction
            Destroy(gameObject, hitSound.length); // Destroy the GameObject after the sound finishes playing
        }

        if (!hasHit && other.gameObject.CompareTag("Missile"))
        {
            Destroy(gameObject);
        }

        if (!hasHit && other.gameObject.CompareTag("Enemy")){
            HealthController enemyHealth = other.gameObject.GetComponent<HealthController>();
            enemyHealth.damageTaken(damage);
            audioSource.PlayOneShot(alienHitSound);
            hasHit = true;
            rb.velocity = Vector2.zero; // Stop the bullet's movement
            coll.enabled = false; // Disable the collider
            spriteRenderer.enabled = false; // Disable the sprite renderer
            // Optionally, you can also add particle effects, visual effects, or other actions here
            
            // Delayed destruction
            Destroy(gameObject, hitSound.length); // Destroy the GameObject after the sound finishes playing
        }
    }
}

