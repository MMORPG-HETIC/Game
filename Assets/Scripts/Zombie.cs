using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private int zombieID;
    private NavMeshAgent agent;
    private GameObject player;
    public int maxHealth = 100;
    private int currentHealth;
    public int damagePerAttack = 20;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    private PlayerHealth playerHealth;
    private float lastAttackTime;
    private PlayerFinder playerFinder;

    //private void Awake()
    //{
    //    if (!Globals.IsServer)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    void Start()
    {
        

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
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }

        if (playerHealth == null || playerHealth.isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }
    }

    public bool TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void AttackPlayer()
    {
        playerHealth.TakeDamage(damagePerAttack);
        lastAttackTime = Time.time;
    }
}
