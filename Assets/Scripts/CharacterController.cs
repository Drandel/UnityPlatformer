using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Vector3 velocity;
    public float walkAcceleration = 5.0f;
    public float groundDeceleration = 10.0f;
    public float maxSpeed = 40f;
    public float speed = 1f;
    public float jumpAcceleration = 25.0f;
    private bool grounded = false;
    public Rigidbody2D rb;
    



    void Start()
    {
    rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate(){ // using fixed update for physics reasons
        float moveInput = Input.GetAxisRaw("Horizontal");

        handlePlayerLookDirection(moveInput);


        if (moveInput != 0)
        {
            rb.AddForce(new Vector2(moveInput * walkAcceleration * Time.deltaTime,0.0f),ForceMode2D.Impulse);
            // Debug.Log("Walk: " +  velocity.x);
        }
        else
        {
        }
        moveInput = Input.GetAxisRaw("Vertical");
        if (moveInput != 0 && grounded)
        {
            rb.AddForce(new Vector2(0.0f,1.0f) * jumpAcceleration, ForceMode2D.Impulse);
            grounded = false;
        }
    }

    private void handlePlayerLookDirection(float moveInput)
    {
        if(moveInput > 0){ // if moving right
                // rotate character along y axis to face right
                Vector3 rotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotation); 
        } else if(moveInput < 0) { // moving left
                // rotate character along y axis to face left
                Vector3 rotation = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotation); 
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log(col.gameObject.tag);
        if(col.gameObject.CompareTag("Ground")){
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if(col.gameObject.CompareTag("Ground")){
            grounded = false;
        }
    }
}
