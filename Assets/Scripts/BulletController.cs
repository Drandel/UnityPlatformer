using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
private void OnCollisionEnter2D(Collision2D other) {
        // // Check if the projectile collides with something
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}