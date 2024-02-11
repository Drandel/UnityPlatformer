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
    Animator anim;
    



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damageResponse(Vector2 contactPoint){
        if (contactPoint.x > transform.position.x){
            rb.AddForce(new Vector2(-20,0.0f),ForceMode2D.Impulse);
        }else if (contactPoint.x < transform.position.x){
            rb.AddForce(new Vector2(20,0.0f),ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate(){ // using fixed update for physics reasons
        float moveInput = Input.GetAxisRaw("Horizontal");

        handlePlayerLookDirection();


        if (moveInput != 0)
        {
            anim.SetBool("isWalking", true);
            rb.AddForce(new Vector2(moveInput * walkAcceleration * Time.deltaTime,0.0f),ForceMode2D.Impulse);
            // Debug.Log("Walk: " +  velocity.x);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        moveInput = Input.GetAxisRaw("Vertical");
        if (moveInput != 0 && grounded)
        {
            rb.AddForce(new Vector2(0.0f,1.0f) * jumpAcceleration, ForceMode2D.Impulse);
            grounded = false;
            anim.SetBool("isFalling", true);
        }
    }

    private void handlePlayerLookDirection()
    {

        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x){ // if moving right
                // rotate character along y axis to face right
                Vector3 rotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotation); 
        } else if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x) { // moving left
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
            anim.SetBool("isFalling", false);
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if(col.gameObject.CompareTag("Ground")){
            grounded = false;
            anim.SetBool("isFalling", true);
        }
    }
}
