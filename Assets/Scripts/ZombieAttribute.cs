using UnityEngine;

public class ZombieAttribute : MonoBehaviour
{
    public ServerManager serverManager;
    public string ID;
    private float SendPositionTimeout = -1;

    private void Start()
    {
        serverManager = FindFirstObjectByType<ServerManager>();
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(ID)) { return; }

        if (Time.time > SendPositionTimeout)
        {
            PayloadZombieStatus status = new PayloadZombieStatus
            {
                id = ID
            };

            status.SetPosition(transform.position);
            status.SetRotation(transform.rotation);

            serverManager.BroadcastUDPMessage(6, status);

            SendPositionTimeout = Time.time + 0.06f;
        }
    }
}
