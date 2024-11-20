using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 20; // Quantité de dégâts infligés par le projectile
    private ScoreManager scoreManager; // Référence au gestionnaire de score

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>(); // Trouver le gestionnaire de score dans la scène
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>(); // Récupérer le composant Enemy de l'objet touché par le projectile

        if (enemy != null && collision.gameObject.CompareTag("Enemy")) // Vérifier si l'objet touché est un ennemi
        {
            if (enemy.TakeDamage(damageAmount)) // Si l'objet touché est un ennemi et que TakeDamage retourne true (signifiant que l'ennemi est tué)
            {
                if (scoreManager != null) // Vérifier si le gestionnaire de score est disponible
                {
                    scoreManager.IncrementScore(); // Incrémenter le score
                }
            }
        }

        Destroy(gameObject); // Détruire le projectile après avoir touché un objet
    }
}
