using UnityEngine;
using UnityEngine.AI;

public class ZombieAttribute : MonoBehaviour
{
    public ServerManager serverManager;
    public string ID;
    private float SendPositionTimeout = -1;
    private PlayerFinder playerFinder;
    private NavMeshAgent agent;
    private GameObject playerToAttack;

    private void Start()
    {
        serverManager = FindFirstObjectByType<ServerManager>();
        playerFinder = GameObject.FindFirstObjectByType<PlayerFinder>();
        playerToAttack = playerFinder.FindPlayerByID(ID);
        agent = GetComponent<NavMeshAgent>();
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
