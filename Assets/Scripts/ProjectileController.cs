using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public int damage = 1;
    public Transform target;
    public GameObject explosionEffect;
    private Rigidbody2D rb;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotationAmount = Vector3.Cross(direction, transform.right).z;

        rb.angularVelocity = -rotationAmount * rotateSpeed;

        rb.velocity = transform.right * speed;
    }

    // void Update()
    // {
    //     transform.Translate(Vector3.right * speed * Time.deltaTime);
    // }

    private void OnCollisionEnter2D(Collision2D other) {
        // // Check if the projectile collides with something
        if (other.gameObject.CompareTag("Player"))
        {
            // ToDo Deal damage to the player and explode
            // Destroy the projectile
            explode();
            Destroy(gameObject);
        }

        // Check if the projectile collides with ground
        if (other.gameObject.CompareTag("Ground"))
        {
            // Destroy the projectile
            explode();
            Destroy(gameObject);
        }
    }

    private void explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }
}
