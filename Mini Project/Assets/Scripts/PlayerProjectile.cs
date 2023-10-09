using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Sprite[] projectileSprites; // This is an array now
    public float speed = 10.0f;
    public float lifetime = 2.0f;

    public int damageAmount = 25; // Amount of damage the projectile deals

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    private float startTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        // Choose a random sprite from the array and assign
        if (projectileSprites.Length > 0)
        {
            spriteRenderer.sprite = projectileSprites[Random.Range(0, projectileSprites.Length)];
        }
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        // Check if the projectile's lifetime has expired
        if (Time.time - startTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(int direction)
    {
        // Adjust the projectile's velocity based on the direction and speed
        rb2d.velocity = new Vector2(Mathf.Abs(speed) * direction, 0);

        // Adjust the projectile orientation based on the direction specified
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * direction; // Ensure the scale's magnitude remains the same
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the projectile hits an object with the "Player" tag, don't damage it.
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject); // Destroy the projectile without damaging the player.
            return;
        }

        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
        }

        // Destroy the projectile on collision.
        Destroy(gameObject);
    }


}
