#if UNITY_EDITOR
#endif
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vie maximale du joueur
    public int currentHealth;
    public HealthBar healthBar;
    public Animator animator;
    public GameObject gameOverCanvas; // Référence à l'écran Game Over dans la scène

    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        gameOverCanvas.SetActive(false);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead)
            return;

        currentHealth -= damageAmount;
        healthBar.SetSlider(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
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
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Activer le Canvas de Game Over
        gameOverCanvas.SetActive(true);

        // Arrêter le jeu
        QuitGame();
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
