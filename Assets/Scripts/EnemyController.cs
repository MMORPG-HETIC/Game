using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;

    void Start()
    {
        // Initialisation du NavMesh et localisation du joueur
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Suivre le joueur
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}