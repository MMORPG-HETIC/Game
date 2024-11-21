using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public int damageAmount = 3; // Montant des dégâts infligés au joueur
    public float attackRange = 3f; // Portée de l'attaque

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
        if (player != null)
        {
            // Calculer la distance entre le zombie et le joueur
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Si le joueur est à portée d'attaque, lui infliger des dégâts
            if (distanceToPlayer <= attackRange)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
