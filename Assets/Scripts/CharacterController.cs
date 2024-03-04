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
    private bool isJumping = false;
    private bool isTimerRunning = false;
    private float airborneTime = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        pauseController = GameObject.Find("PauseCanvas").GetComponent<PauseMenuController>();
    }

    // Another attempt at fixed the "not grounded when should be bug". Basically resets isGrounded to true after 10 seconds of not being grounded
    void Update()
    {
        if (isJumping && !grounded)
        {
            airborneTime += Time.deltaTime;
            Debug.Log(airborneTime);
            if (airborneTime >= 10f) setGrounded(true);;
        }
        else airborneTime = 0f;
    }

    public void damageResponse(Vector2 contactPoint){
        float knockbackDirection = contactPoint.x > transform.position.x ? damageKnockback * -1 : damageKnockback;
        rb.AddForce(new Vector2(knockbackDirection,0.0f),ForceMode2D.Impulse);
    }

    private void FixedUpdate(){ // using fixed update for physics reasons
        if(!cutScene){
        float moveInput = Input.GetAxisRaw("Horizontal");
        
        handlePlayerLookDirection();
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            anim.SetBool("isWalking", true);
            rb.AddForce(new Vector2(moveInput * walkAcceleration * Time.deltaTime,0.0f),ForceMode2D.Impulse);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        bool jumpInput = Input.GetKey(KeyCode.Space);
        if (jumpInput && grounded)
        {
            rb.AddForce(new Vector2(0.0f,1.0f) * jumpAcceleration, ForceMode2D.Impulse);
            setGrounded(false);
            isJumping = true;
            isTimerRunning = true;
            audioSource.PlayOneShot(jumpSound);
        }
      }else{
        runCutScene();
      }
        
    }

    private void handlePlayerLookDirection()
    {
        float angle = Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x ? 0f : 180f;
        Vector3 rotation = new Vector3(transform.rotation.x, angle, transform.rotation.z);
        transform.rotation = Quaternion.Euler(rotation); 
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        if(col.gameObject.CompareTag("Ground")){
            setGrounded(true);
            isTimerRunning = false; // Reset timer when the player lands
        }
        else if(col.gameObject.CompareTag("RocketPickup")){
            Transform childTransform = transform.GetChild(1);
            childTransform.GetComponent<Renderer>().enabled = true; 
            Destroy(col.gameObject);
        }
    }  

    private void OnCollisionExit2D(Collision2D col) {
        if(col.gameObject.CompareTag("Ground")) setGrounded(false);
    }

    // OnCollisionStay2D fires when the player is making continuous collision with something
    // If this fires, and the collision target is the ground, we set grounded = true
    // Had to add an "isJumping" flag to prevent this code from executing the same frame that the player jumps, resulting in a double jump
    // This was the best fix I could come up with for a nasty bug that caused they player to be stuck in a state where they weren't grounded, even if they were flat on the ground. 
    private void OnCollisionStay2D(Collision2D col) {
        if(col.gameObject.CompareTag("Ground") && !isJumping){
            isJumping = false;
            setGrounded(true);
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

    private void setGrounded(bool state){
        grounded = state;
        anim.SetBool("isFalling", !state);
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
