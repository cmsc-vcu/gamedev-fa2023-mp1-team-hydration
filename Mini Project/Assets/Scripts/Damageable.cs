using UnityEngine;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private float invincibilityDuration = 0.25f; // Duration of invincibility after being hit

    private Animator animator;
    private int currentHealth;
    private bool isAlive = true;
    private bool isInvincible = false; // To keep track of invincibility status

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive || isInvincible) return; // Do not process damage if not alive or invincible

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        StartInvincibility(); // Start invincibility after taking damage

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        Invoke(nameof(EndInvincibility), invincibilityDuration);
    }

    private void EndInvincibility()
    {
        isInvincible = false;
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log(gameObject.name + " has been destroyed!");

        if (gameObject.CompareTag("Player"))
        {
            RestartScene();
            return;  // No need to continue the rest of the code for player death.
        }

        animator.SetBool("isDead", true);
        Destroy(gameObject); // Destroy the game object
    }

    private void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Getter methods for external scripts to check health and alive status
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}
