using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float patrolRadius = 10f;
    private Vector2 rightPoint;
    private Vector2 leftPoint;
    private bool movingRight = false;
    private Rigidbody2D rb;

    private enum BossState { Waiting, Active, CutScene }
    private BossState currentState;
    public float damage = 10f;

    public float fractionWiggle = .20f;
    public GameObject projectilePrefab;
    public GameObject enemyPrefab;
    public List<Transform> firePoints;
    public int[] spawnPoints = {0,8};
    public float fireRate = 2f;
    private float FireTime = 0f;
    public float startPos = 27.0f;
    public GameObject BossUI;
    public GameObject aggroIcon;

    public GameObject explosionEffect;
    public bool atStartPos = false;
    bool isQuitting = false;

    public GameObject levelCompleteText;
    private LevelCompleteAnimation levelCompleteAnimation;
    private GameStateController gameState;
    void Start()
    {
        if(GameObject.Find("GameState") != null){
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        }
        levelCompleteAnimation = levelCompleteText.GetComponent<LevelCompleteAnimation>();
        leftPoint = (Vector2)transform.position - Vector2.right * patrolRadius;
        rightPoint = (Vector2)transform.position + Vector2.right * patrolRadius;
        
        currentState = BossState.Waiting;
        rb = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform.transform.Find("FirePoints"))
        {
            firePoints.Add(child);
        }
        FireTime = Time.time;
        StartCoroutine(StateMachine());
    }

    private void FixedUpdate() 
    {
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case BossState.Waiting:
                    yield return null;    
                    break;
                case BossState.CutScene:
                    yield return CutScene();
                    break;
                case BossState.Active:
                    yield return Active();    
                break;
        }
    }
    }

    
    IEnumerator Active()
{
        Vector2 targetPoint = movingRight ? rightPoint : leftPoint;
        targetPoint.y = transform.position.y;

        // Calculate move direction without gravity
        Vector2 moveDirection = (targetPoint - (Vector2)transform.position).normalized;
        moveDirection.y =   -Mathf.Sin(Math.Abs(targetPoint.x -transform.position.x)/(patrolRadius) * MathF.PI) * 0.5f;


        // Calculate the total velocity including both movement and gravity
        Vector2 totalVelocity = moveDirection * patrolSpeed;

        // Move the enemy
        rb.velocity = totalVelocity;

        float distanceToTarget = Vector2.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.1f)
        {
            rb.velocity = Vector2.zero;
            movingRight = !movingRight;
            yield break;
        }
        if(Time.time -  FireTime >= fireRate){
            LaunchMissle();
            FireTime = Time.time;
        }

        yield return null;
}

    IEnumerator CutScene()
{

    Debug.Log("UFO CutScene");
    if(transform.position.y >= startPos){
        Vector2 totalVelocity = new Vector2(0,-patrolSpeed);
        rb.velocity = totalVelocity;
        Debug.Log("Alien Velocity" + rb.velocity);
    } else{
       atStartPos = true;
       Vector2 totalVelocity = new Vector2(0,0);
       Transform allertpoint = transform.Find("AllertPoint");
       GameObject icon = Instantiate(aggroIcon, allertpoint.position, aggroIcon.transform.rotation);
       icon.GetComponent<AggroIconController>().startingScale =  new Vector3(0.1f, 0.1f, 0.1f);
       icon.GetComponent<AggroIconController>().middleScale = new Vector3(8f, 8f, 8f);
       icon.GetComponent<AggroIconController>().endingScale = new Vector3(4f, 4f, 4f);
       rb.velocity = totalVelocity;
       currentState = BossState.Waiting;

    }
    yield return null;
}

void LaunchMissle(){
   
   int firePointIndex = UnityEngine.Random.Range(0,firePoints.Count);
   Transform firePoint = firePoints[firePointIndex]; 
   if(spawnPoints.Contains(firePointIndex)){
    GameObject spawn = Instantiate(enemyPrefab, firePoint.position, Quaternion.identity);
    spawn.GetComponent<Enemy>().startState = Enemy.EnemyState.Deploying;
    spawn.GetComponent<Enemy>().detectionRadius = 100.0f;
    spawn.GetComponent<Enemy>().engageRadius  = 100.0f;
    spawn.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous; 
   }else{
    GameObject missle = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    missle.GetComponent<Transform>().localScale = new Vector3(.3f, .3f, 1);
   }
}
    

    

    

    
    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log(col.gameObject.tag);
        if(col.gameObject.CompareTag("Player")){
            col.gameObject.GetComponent<HealthController>().damageTaken(damage);
            col.gameObject.GetComponent<CharacterController>().damageResponse(col.contacts[0].point);
        }
    }

    public void alert(){
     currentState = BossState.Active;
     BossUI.GetComponent<Canvas>().enabled = true;
    }
    public void startCutSCene(){
     Debug.Log("Start");
     currentState = BossState.CutScene;
    }
    
    void OnDestroy()
    {
        if (!isQuitting && !PauseMenuController.IsPaused)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            levelCompleteAnimation.ShowLevelComplete();
            if(gameState != null){
            gameState.LevelComplete();
            }
            // play fireworks
        }
    }


     void OnApplicationQuit()
    {
        isQuitting = true;
    }

}
    

