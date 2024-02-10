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
    private Status status = Status.Patrolling; 
    public LayerMask playerLayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointA.transform;
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

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PatrolPoint")){
            if(status == Status.Patrolling)
            {
                currentPoint = currentPoint == pointA.transform ? pointB.transform : pointA.transform;
                canMove = false;
                StartCoroutine(waitCoroutine());
            }
        }

        if(other.CompareTag("Player")){
            Debug.Log("Attacking!");
            status = Status.Attacking;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log("Patrolling!");
            status = Status.Patrolling;
        }
    }


    IEnumerator waitCoroutine(){
        yield return new WaitForSeconds(2);
        canMove = true;
        var scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }
}

enum Status {
    Patrolling,
    Attacking
}