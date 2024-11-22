using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public int damageAmount = 3; // Montant des dégâts infligés au joueur
    public float attackRange = 3f; // Portée de l'attaque

    private GameObject player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("Le joueur n'a pas de composant PlayerHealth !");
            }
        }
        else
        {
            Debug.LogError("Aucun joueur trouvé !");
        }
    }

    void Update()
    {
        if (playerHealth == null || playerHealth.IsDead()) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        playerHealth.TakeDamage(damagePerAttack);
        lastAttackTime = Time.time;

        Debug.Log($"Zombie attaque pour {damagePerAttack} dégâts. Santé restante du joueur : {playerHealth.CurrentHealth}");
    }
}