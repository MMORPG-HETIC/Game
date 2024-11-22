using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 250; // Points de vie maximum de l'ennemi
    private int currentHealth; // Points de vie actuels de l'ennemi

    void Start()
    {
        currentHealth = maxHealth; // Au début, les points de vie actuels sont égaux aux points de vie maximum
    }

    public bool TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Réduire les points de vie actuels de l'ennemi en fonction des dégâts subis

        if (currentHealth <= 0)
        {
            Die(); // Si les points de vie actuels sont inférieurs ou égaux à zéro, l'ennemi meurt
            return true; // Renvoie true pour indiquer que l'ennemi a été tué
        }
        
        return false; // Renvoie false pour indiquer que l'ennemi est toujours en vie
    }

    void Die()
    {
        // Ajoutez ici toute logique nécessaire pour faire disparaître l'ennemi (comme la destruction de l'objet)
        Destroy(gameObject);
    }
}
