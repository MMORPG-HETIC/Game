using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        // Initialiser la santé de l'ennemi
        currentHealth = maxHealth;
    }

    public bool TakeDamage(int damageAmount)
    {
        // Réduire la santé et vérifier si l'ennemi est tué
        currentHealth -= damageAmount;
        Debug.Log($"Dégâts infligés : {damageAmount}, Santé restante : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Die()
    {
        Debug.Log("Ennemi éliminé !");
        Destroy(gameObject);
    }
}