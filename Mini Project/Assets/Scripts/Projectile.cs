using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10.0f; // Adjust the speed of the projectile
    public float lifetime = 2.0f; // Time until the projectile disappears

    public int damageAmount = 10; // Amount of damage the projectile deals


    private float startTime;
    private Vector3 direction; // Direction to move the projectile

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        // Move the projectile forward
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        // Check if the projectile's lifetime has expired
        if (Time.time - startTime >= lifetime)
        {
            Destroy(gameObject); // Destroy the projectile after its lifetime
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Handle collision with other objects here (if needed)
        // For example, you can check the collision object's tag or layer.
        // Destroy the projectile on collision

        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
        }

        Destroy(gameObject);
    }
}
