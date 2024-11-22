using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Configuration")]
    public int maxHealth = 100; // Santé maximale
    private int currentHealth; // Santé actuelle
    public GameObject gameOverCanvas; // Canvas pour le joueur principal
    public Animator animator;

    [HideInInspector]
    public HealthBar healthBar; // Barre de santé pour le joueur principal

    private bool isDead;
    private bool isMainPlayer; // Indique si ce joueur est le joueur principal

    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (isMainPlayer && healthBar != null)
        {
            healthBar.SetSliderMax(maxHealth);
            healthBar.SetSlider(currentHealth);
        }

        if (isMainPlayer && gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    public void Initialize(bool isMainPlayer, HealthBar healthBar = null)
    {
        this.isMainPlayer = isMainPlayer;

        if (isMainPlayer && healthBar != null)
        {
            this.healthBar = healthBar;
        }

        currentHealth = maxHealth; // S'assurer que la santé est initialisée
        isDead = false; // Réinitialiser l'état de mort
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if (isMainPlayer && healthBar != null)
        {
            healthBar.SetSlider(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);

        if (isMainPlayer && healthBar != null)
        {
            healthBar.SetSlider(currentHealth);
        }
    }

    public void HealFull()
    {
        Heal(maxHealth - currentHealth);
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (isMainPlayer && gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Debug.Log($"{name} est mort.");

        Destroy(gameObject, 2f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
