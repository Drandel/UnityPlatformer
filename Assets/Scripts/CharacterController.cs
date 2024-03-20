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
    public float damageKnockback = 6.0f;
    private bool grounded = false;
    public Rigidbody2D rb;
    Animator anim;
    private AudioSource audioSource;
    public AudioClip jumpSound;
    Vector3 respawnPoint;
    private GameStateController gameState;
    private PauseMenuController pauseController;
    public bool cutScene = false;
    private float cutSceneWalkDistance = 30.0f;
    public bool walkDone = false;
    private float cutSceneStart;
    private bool AlieninPosition = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        pauseController = GameObject.Find("PauseCanvas").GetComponent<PauseMenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damageResponse(Vector2 contactPoint){
        if (contactPoint.x > transform.position.x){
            rb.AddForce(new Vector2(-damageKnockback,0.0f),ForceMode2D.Impulse);
        }else if (contactPoint.x < transform.position.x){
            rb.AddForce(new Vector2(damageKnockback,0.0f),ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate(){ // using fixed update for physics reasons
        if(!cutScene){
        float moveInput = Input.GetAxisRaw("Horizontal");
        
        handlePlayerLookDirection();


        if (moveInput != 0)
        {
            anim.SetBool("isWalking", true);
            rb.AddForce(new Vector2(moveInput * walkAcceleration * Time.deltaTime,0.0f),ForceMode2D.Impulse);
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
            audioSource.PlayOneShot(jumpSound);
        }
      }else{
        runCutScene();
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
        
        if(col.gameObject.CompareTag("Ground")){
            grounded = true;
            anim.SetBool("isFalling", false);
        }
        else if(col.gameObject.CompareTag("RocketPickup")){
            Transform childTransform = transform.GetChild(1);
            childTransform.GetComponent<Renderer>().enabled = true; 
            Destroy(col.gameObject);
        }
    }  

    private void OnCollisionExit2D(Collision2D col) {
        if(col.gameObject.CompareTag("Ground")){
            grounded = false;
            anim.SetBool("isFalling", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Checkpoint")){
            respawnPoint = transform.position;

        }

        if(col.gameObject.CompareTag("FallZone")){
            DieAndRespawn();
        }
    }

    public void DieAndRespawn()
    {
        if(gameState.lifeCount > 0){
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            transform.position = respawnPoint;
            gameState.lifeCount -= 1;
        } else {
            // game over
            pauseController.Pause(true);
            // Destroy(gameObject);
        }
        
        
    }
    public void BossCutScene(){
        cutScene = true;
        cutSceneStart = transform.position.x;
        Vector3 rotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(rotation); 
    }
    private void runCutScene(){
        
        if(transform.position.x < cutSceneStart + cutSceneWalkDistance){
        anim.SetBool("isWalking", true);
        rb.AddForce(new Vector2(1 * walkAcceleration * Time.deltaTime,0.0f),ForceMode2D.Impulse);
        }else if(!AlieninPosition){
            walkDone = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isStatic", true);
        }else{
            anim.SetBool("isStatic", false);
            anim.SetBool("isLookUp", true);
        }
        
    }

    public void lookUp(){
        AlieninPosition = true;
    }
    public void endCutScene(){
        cutScene = false;
        anim.SetBool("isLookUp", false);
    }
}
