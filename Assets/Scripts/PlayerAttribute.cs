using System.Net;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public ClientManager clientManager;
    public string ID;
    private float SendPositionTimeout = -1;
    private PlayerFinder playerFinder;

    private void Start()
    {
        clientManager = FindFirstObjectByType<ClientManager>();
        playerFinder = FindFirstObjectByType<PlayerFinder>();
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(ID)) { return; }

        if (Time.time > SendPositionTimeout)
        {
            GameObject Player = playerFinder.FindPlayerByIP(ID);
            PayloadPlayerStatus status = new PayloadPlayerStatus { id = ID };
            status.SetPosition(Player.transform.position);
            clientManager.SendServerUDPMessage(3, status);
            SendPositionTimeout = Time.time + 0.06f;
        }
    }
}
