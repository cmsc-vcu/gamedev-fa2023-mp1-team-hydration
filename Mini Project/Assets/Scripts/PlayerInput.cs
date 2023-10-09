using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Required for scene management


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpp = 10f;
    public GameObject Player_Projectile; // The projectile you want to shoot
    public Transform FP; // The position where bullets should be spawned
    private float lastShootTime; // Record the last time the enemy shot


    private bool canFire = true;
    public float fireCooldown = 0.5f;

    Vector2 moveInput;

    Rigidbody2D RB;
    Animator animator;
    SpriteRenderer spriteRenderer;
    TouchingDirections TD;

    [SerializeField] private bool _isMoving = false;

    public bool isMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", _isMoving);
        }
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TD = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        RB.velocity = new Vector2(moveInput.x * speed, RB.velocity.y);

        if (RB.velocity.x > 0)
        {
            spriteRenderer.flipX = false; // Facing right
        }
        else if (RB.velocity.x < 0)
        {
            spriteRenderer.flipX = true; // Facing left
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider's tag is "EndGameItem" or another appropriate tag
        if (other.CompareTag("EndGameItem"))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        isMoving = moveInput != Vector2.zero;
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started && TD.IsGrounded)
        {
            animator.SetTrigger("jump");
            RB.velocity = new Vector2(RB.velocity.x, jumpp);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && canFire)
        {
            Fire();
            animator.SetTrigger("onFire");
            canFire = false;
            Invoke("ResetFireCooldown", fireCooldown);
        }
    }

    void Fire()
    {
        // Instantiate the projectile at FP's position
        GameObject projectileInstance = Instantiate(Player_Projectile, FP.position, Quaternion.identity);
        PlayerProjectile projectileScript = projectileInstance.GetComponent<PlayerProjectile>();

        // If the player is facing left, set the projectile direction to left
        if (spriteRenderer.flipX) // Use spriteRenderer's flipX to determine direction
        {
            projectileScript.SetDirection(-1);
        }
        else
        {
            projectileScript.SetDirection(1);
        }
    }


    private void ResetFireCooldown()
    {
        canFire = true;
    }
}
