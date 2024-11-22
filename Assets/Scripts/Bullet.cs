using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 20; // Quantité de dégâts infligés par le projectile
    private ScoreManager scoreManager; // Référence au gestionnaire de score

    void Start()
    {
        // Trouver le ScoreManager dans la scène
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();

        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager introuvable !");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Vérifie si la balle touche un ennemi
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null && collision.gameObject.CompareTag("Enemy"))
        {
            // Infliger des dégâts et incrémenter le score si l'ennemi est tué
            if (enemy.TakeDamage(damageAmount) && scoreManager != null)
            {
                scoreManager.IncrementScore();
            }
        }

        // Détruire la balle après la collision
        Destroy(gameObject, 0.5f);
    }
}