using UnityEngine;

public class EnemyTrackingPlayer : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float stopDistance = 2.0f;
    public float detectionRange = 5.0f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 2.0f;

    private Transform player;
    private Rigidbody2D rb;
    private bool playerDetected = false;
    private float lastShootTime;

    public float patrolSpeed = 1.5f;
    private int patrolDirection = 1; // 1 is right, -1 is left
    private bool isAtEdge;
    private bool isAtWall;

    private void Awake()
    {
        FindPlayer();
        rb = GetComponent<Rigidbody2D>();
        lastShootTime = -shootCooldown;
        transform.localScale = new Vector3(patrolDirection, 1f, 1f);
    }

    private void Update()
    {
        CheckEnvironment();

        // If player is null, try to find it again and exit the function
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            playerDetected = true;
        }

        if (playerDetected)
        {
            HandlePlayerDetection(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    private void HandlePlayerDetection(float distanceToPlayer)
    {
        if (distanceToPlayer > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Check if the direction the enemy is moving towards has ground
            bool canMoveInDirection = true;
            if (patrolDirection == 1 && isAtEdge && direction.x > 0) // If enemy is looking right and there's an edge, and player is to the right
            {
                canMoveInDirection = false;
            }
            else if (patrolDirection == -1 && isAtEdge && direction.x < 0) // If enemy is looking left and there's an edge, and player is to the left
            {
                canMoveInDirection = false;
            }

            // Only move if it's safe
            if (canMoveInDirection)
            {
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                rb.velocity = Vector2.zero; // Stop if there's an edge
            }
        }
        else
        {
            rb.velocity = Vector2.zero;

            if (Time.time - lastShootTime >= shootCooldown)
            {
                Shoot();
            }
        }
    }


    private void Patrol()
    {
        rb.velocity = new Vector2(patrolDirection * patrolSpeed, rb.velocity.y);

        // If enemy detects a cliff or a wall, switch patrol direction
        if (isAtEdge || isAtWall)
        {
            SwitchPatrolDirection();
        }
    }

    private void CheckEnvironment()
    {
        // Cliff Detection
        Vector2 cliffRayOrigin = (patrolDirection == 1) ? new Vector2(transform.position.x + 0.5f, transform.position.y) : new Vector2(transform.position.x - 0.5f, transform.position.y);
        RaycastHit2D groundInfo = Physics2D.Raycast(cliffRayOrigin, Vector2.down, 2f, LayerMask.GetMask("Ground"));
        isAtEdge = (groundInfo.collider == null);

        // Wall Detection
        Vector2 wallRayOrigin = transform.position;
        RaycastHit2D wallInfo = Physics2D.Raycast(wallRayOrigin, Vector2.right * patrolDirection, 0.6f, LayerMask.GetMask("Ground"));
        isAtWall = (wallInfo.collider != null);
    }

    private void SwitchPatrolDirection()
    {
        patrolDirection = -patrolDirection;
        transform.localScale = new Vector3(patrolDirection, 1f, 1f);
    }

    private void Shoot()
    {
        GameObject newProjectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDirection(player.position - firePoint.position);
        lastShootTime = Time.time;
    }

    private void FindPlayer()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            player = playerObject.transform;
        }
    }
}
