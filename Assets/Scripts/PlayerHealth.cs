using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Configuration")]
    public int maxHealth = 100;
    private int currentHealth;
    private GameObject gameOverCanvas; // Canvas pour le joueur principal
    public Animator animator;

    [HideInInspector]
    public HealthBar healthBar;

    private bool isDead;
    private bool isMainPlayer;

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

    public void Initialize(bool isMainPlayer, HealthBar healthBar = null, GameObject gameOverCanvas = null)
    {
        this.isMainPlayer = isMainPlayer;

        if (isMainPlayer)
        {
            this.healthBar = healthBar;
            this.gameOverCanvas = gameOverCanvas;
        }

        currentHealth = maxHealth;
        isDead = false;
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

        // Détacher la caméra principale
        Camera mainCamera = Camera.main;
        if (mainCamera != null && mainCamera.transform.parent == transform)
        {
            mainCamera.transform.SetParent(null); // Détache la caméra
            ThirdPersonOrbitCamBasic camScript = mainCamera.GetComponent<ThirdPersonOrbitCamBasic>();
            if (camScript != null)
            {
                camScript.player = null; // Retire la cible pour éviter l'erreur
            }
        }

        // Activer le GameOver Canvas
        if (isMainPlayer && gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Debug.Log($"{name} est mort.");

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
