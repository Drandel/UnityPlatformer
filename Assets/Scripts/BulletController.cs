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
    public float maxLifeTime = 8f; 

    private void Start() {
        StartCoroutine(DestroyAfterDelay(8f));
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shootSound);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!hasHit && other.gameObject.CompareTag("Ground"))
        {
            audioSource.PlayOneShot(hitSound);
            hasHit = true;
            rb.velocity = Vector2.zero;
            coll.enabled = false; 
            spriteRenderer.enabled = false;
            Destroy(gameObject, hitSound.length);
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
            rb.velocity = Vector2.zero;
            coll.enabled = false;
            spriteRenderer.enabled = false;
            Destroy(gameObject, hitSound.length);
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

