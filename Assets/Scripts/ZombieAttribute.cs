using UnityEngine;
using UnityEngine.AI;

public class ZombieAttribute : MonoBehaviour
{
    public ServerManager serverManager;
    public ClientManager clientManager;
    public string ID;
    private float SendPositionTimeout = -1;
    private PlayerFinder playerFinder;
    private NavMeshAgent agent;
    private GameObject playerToAttack;
    public int maxHealth = 100;
    private int currentHealth;
    public int damagePerAttack = 20;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;

    private void Start()
    {
        serverManager = FindFirstObjectByType<ServerManager>();
        playerFinder = GameObject.FindFirstObjectByType<PlayerFinder>();
        playerToAttack = playerFinder.FindPlayerByID(ID);
        agent = GetComponent<NavMeshAgent>();
    }

    public bool TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"D?g?ts inflig?s : {damageAmount}, Sant? restante : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Die()
    {
        if (Time.time > SendPositionTimeout && serverManager)
        {
            PayloadCheck die = new PayloadCheck { id = ID};
            clientManager.SendServerUDPMessage(6, die);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(ID)) { return; }

        if (Time.time > SendPositionTimeout && serverManager)
        {
            if (playerToAttack != null)
            {
                agent.SetDestination(playerToAttack.transform.position);
            }
            PayloadZombieStatus status = new PayloadZombieStatus
            {
                id = ID
            };

            status.SetPosition(transform.position);
            status.SetRotation(transform.rotation);

            serverManager.BroadcastUDPMessage(5, status);

            SendPositionTimeout = Time.time + 0.06f;
        }
    }
}
