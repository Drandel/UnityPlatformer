using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float patrolRadius = 10f;
    public float detectionRadius = 5f;
    public float pauseDuration = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 leftPoint;
    private Vector2 rightPoint;
    private bool movingRight = false;

    private enum EnemyState { Patrolling, Pausing, Attacking }
    private EnemyState currentState;
    public float Damage = 10f;
    Animator anim;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

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
                    yield return null;
                    break;
            }
        }
    }

    private void flipCharacter()
    {
        if(transform.rotation.y == 180f){
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
            Vector2 moveDirection = (targetPoint - (Vector2)transform.position).normalized;
            rb.velocity = moveDirection * patrolSpeed;

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
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            currentState = EnemyState.Attacking;
            Debug.Log("Player detected!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log(col.gameObject.tag);
        if(col.gameObject.CompareTag("Player")){
            col.gameObject.GetComponent<HealthController>().damageTaken(Damage);
            col.gameObject.GetComponent<CharacterController>().damageResponse(col.contacts[0].point);
        }
    }
}
