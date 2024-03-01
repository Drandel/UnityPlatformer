using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float patrolRadius = 10f;
    public float detectionRadius = 5f;
    public float maxDetectionHeight = 5f;
    public float engageRadius = 5f;
    public float pauseDuration = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 leftPoint;
    private Vector2 rightPoint;
    private bool movingRight = false;

    private enum EnemyState { Patrolling, Pausing, Attacking }
    private EnemyState currentState;
    public float damage = 10f;
    Animator anim;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float nextFireTime = 0f;
    public GameObject explosionEffect;
    private bool isQuitting = false;
    public GameObject aggroIcon;
    public float gravity = 9.8f; // Gravity strength
    public float groundDistance = 0.1f; // Distance to check for ground
    public LayerMask groundMask; // Layer mask for the ground objects
    public bool isGrounded; // Flag to check if the object is grounded
    public float gravityMultiplier = 1f; // Optional gravity multiplier
    public bool shouldFlip = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        leftPoint = (Vector2)transform.position - Vector2.right * patrolRadius;
        rightPoint = (Vector2)transform.position + Vector2.right * patrolRadius;

        currentState = EnemyState.Patrolling;
        StartCoroutine(StateMachine());
    }

    private void FixedUpdate() 
    {
        ApplyGravity(); // Apply gravity in FixedUpdate
    }

    private void ApplyGravity()
    {
        // Check if the object is grounded
        isGrounded = Physics2D.Raycast(transform.position, -transform.up, groundDistance, groundMask);

        // Apply gravity if not grounded
        if (!isGrounded)
        {
            // Apply gravity force
            Vector3 gravityForce = -transform.up * gravity * gravityMultiplier;
            rb.AddForce(gravityForce, ForceMode2D.Force);
        }
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Patrolling:
                    anim.SetBool("isWalking", true);
                    yield return Patrol();
                    break;
                case EnemyState.Pausing:
                    anim.SetBool("isWalking", false);
                    CheckForPlayer();
                    yield return new WaitForSeconds(pauseDuration);

                    flipCharacter();
                
                    currentState = EnemyState.Patrolling;
                    break;
                case EnemyState.Attacking:
                    // Add logic for attacking
                    anim.SetBool("isWalking", false);
                    rb.velocity = Vector2.zero; // Stop movement when attacking

                    if (Time.time >= nextFireTime)
                    {
                        Shoot();
                        nextFireTime = Time.time + 1f / fireRate;
                    }
                    CheckStillEngaged();
                    yield return null;
                    break;
            }
        }
    }

    private void flipCharacter()
    {
        if(!shouldFlip) return;
        if(transform.rotation.eulerAngles.y == 180f){
            Vector3 rotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
        } else {
            Vector3 rotation = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
        }
        
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    IEnumerator Patrol()
{
    while (currentState == EnemyState.Patrolling)
    {
        Vector2 targetPoint = movingRight ? rightPoint : leftPoint;
        targetPoint.y = transform.position.y;

        // Calculate move direction without gravity
        Vector2 moveDirection = (targetPoint - (Vector2)transform.position).normalized;

        // Apply gravity
        Vector2 gravityForce = Vector2.down * gravity * Time.deltaTime;
        moveDirection += gravityForce;

        // Normalize the move direction again after applying gravity
        moveDirection.Normalize();

        // Calculate the total velocity including both movement and gravity
        Vector2 totalVelocity = moveDirection * patrolSpeed;

        // Move the enemy
        rb.velocity = totalVelocity;

        float distanceToTarget = Vector2.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.1f)
        {
            rb.velocity = Vector2.zero;
            currentState = EnemyState.Pausing;
            movingRight = !movingRight;
            yield break;
        }

        CheckForPlayer(); // Check for player during patrol
        yield return null;
    }
}

    void CheckForPlayer()
    {
        if (
            Vector2.Distance(transform.position, player.position) <= detectionRadius &&
            Mathf.Abs(player.position.y - transform.position.y)  <= maxDetectionHeight
        )
        {
            checkFacingPlayer();
            Instantiate(aggroIcon, firePoint.position, aggroIcon.transform.rotation);
            currentState = EnemyState.Attacking;
        }
    }

    private void checkFacingPlayer()
    {
        if (player.position.x < transform.position.x)
        {
            //player is on left
            if(transform.rotation.eulerAngles.y == 180f){ // face left
                Vector3 rotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotation);
            }
        } else {
            //player is on right
            if(transform.rotation.eulerAngles.y == 0f){ //face right
                Vector3 rotation = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotation);
            }
        }
    }

    void CheckStillEngaged()
    {
        if (Vector2.Distance(transform.position, player.position) >= engageRadius)
        {
            currentState = EnemyState.Patrolling;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);        
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, engageRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxDetectionHeight);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log(col.gameObject.tag);
        if(col.gameObject.CompareTag("Player")){
            col.gameObject.GetComponent<HealthController>().damageTaken(damage);
            col.gameObject.GetComponent<CharacterController>().damageResponse(col.contacts[0].point);
        }
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (!isQuitting && !PauseMenuController.IsPaused)
        {
            if(!gameObject.scene.isLoaded) return;
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
    }

}
