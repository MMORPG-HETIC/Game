using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Configuration")]
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;
    public GameObject gameOverCanvas;
    public Animator animator;

    private bool isDead;

    public int CurrentHealth => currentHealth; // Propriété pour accéder à la santé actuelle

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetSliderMax(maxHealth);
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        

        if (healthBar != null)
        {
            healthBar.SetSlider(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealFull()
    {
        if (isDead) return;

        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetSlider(currentHealth);
        }
    }

    public void Heal(int healAmount)
    {
        if (isDead)
            return;

        currentHealth += healAmount;

        // Assurez-vous que la santé ne dépasse pas le maximum
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Mettre à jour la barre de santé
        healthBar.SetSlider(currentHealth);

        Debug.Log($"Vie restaurée : +{healAmount}. Vie actuelle : {currentHealth}");
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}