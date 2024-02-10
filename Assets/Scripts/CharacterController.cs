using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 1f;
    public float walkAcceleration = 0.2f;
    public float groundDeceleration = 0.5f;
    public float maxSpeed = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            velocity.x = Math.Min(Mathf.MoveTowards(velocity.x, speed * moveInput, walkAcceleration * Time.deltaTime), maxSpeed);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, groundDeceleration * Time.deltaTime);
        }
        transform.Translate(velocity * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag);
        if(col.gameObject.CompareTag("Coin")){
            Destroy(col.gameObject);
        }
    }
}
