using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 20; // Dégâts infligés
    private ScoreManager scoreManager;

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
        ZombieAttribute zombieAttribute = collision.gameObject.GetComponent<ZombieAttribute>();
        if (zombieAttribute != null && collision.gameObject.CompareTag("Enemy"))
        {
            // Infliger des dégâts et incrémenter le score si l'ennemi est tué
            if (zombieAttribute.TakeDamage(damageAmount) && scoreManager != null)
            {
                scoreManager.IncrementScore();
            }
        }

        // Détruire la balle après la collision
        Destroy(gameObject, 0.5f);
    }
}