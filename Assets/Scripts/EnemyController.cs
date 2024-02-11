using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed = 2f;
    public float detectionRadius = 15f;
    private bool canMove = true;
    public Status status = Status.Attacking; 
    public LayerMask playerLayer;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float nextFireTime = 0f;
    Animator anim;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointA.transform;
            anim.SetBool("isWalking", true);
    }

    void Update()
    {
        if(status == Status.Patrolling){
            if(canMove){
                if(currentPoint == pointB.transform )
                {
                    rb.velocity = new Vector2(speed, 0);
                } else {
                    rb.velocity = new Vector2(-speed, 0);
                }
            }
        }

        if(status == Status.Attacking) {
            anim.SetBool("isWalking", false);
            Debug.Log(Time.time >= nextFireTime);
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }

    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PatrolPoint")){
            if(status == Status.Patrolling)
            {
                currentPoint = currentPoint == pointA.transform ? pointB.transform : pointA.transform;
                canMove = false;
                anim.SetBool("isWalking", false);
                StartCoroutine(waitCoroutine());
            }
        }
    }


    IEnumerator waitCoroutine(){
        yield return new WaitForSeconds(2);
        anim.SetBool("isWalking", true);
        canMove = true;
        var scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }
}

public enum Status {
    Patrolling,
    Attacking
}