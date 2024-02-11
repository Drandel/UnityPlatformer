using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float attackSpeed = 4f;
    public float patrolRadius = 10f;
    public float attackRadius = 5f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 patrolPoint;
    private enum EnemyState { Patrolling, Attacking }
    private EnemyState currentState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Patrolling;
        // Initial patrol point
        SetPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                CheckForPlayer();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        // Move towards patrol point
        Vector2 moveDirection = (patrolPoint - (Vector2)transform.position).normalized;
        rb.velocity = moveDirection * patrolSpeed;

        // If reached patrol point, set a new one
        if (Vector2.Distance(transform.position, patrolPoint) < 0.1f)
        {
            SetPatrolPoint();
        }
    }

    void SetPatrolPoint()
    {
        // Randomly set a new patrol point within patrol radius
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float xOffset = Mathf.Cos(randomAngle) * patrolRadius;
        float yOffset = Mathf.Sin(randomAngle) * patrolRadius;
        patrolPoint = (Vector2)transform.position + new Vector2(xOffset, yOffset);
    }

    void CheckForPlayer()
    {
        // If player is within attack radius, switch to attacking state
        if (Vector2.Distance(transform.position, player.position) < attackRadius)
        {
            currentState = EnemyState.Attacking;
        }
    }

    void Attack()
    {
        // Move towards player and attack
        Vector2 moveDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.velocity = moveDirection * attackSpeed;
        // Implement your attacking logic here
    }

    void OnDrawGizmosSelected()
    {
        // Draw patrol radius as blue circle
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        // Draw attack radius as red circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
