using System.Net;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAttribute : MonoBehaviour
{
    public ClientManager clientManager;
    public string ID;
    private float SendPositionTimeout = -1;

    private void Start()
    {
        clientManager = FindFirstObjectByType<ClientManager>();
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(ID)) { return; }

        if (Time.time > SendPositionTimeout)
        {
            PayloadPlayerStatus status = new PayloadPlayerStatus { id = ID };
            status.SetPosition(transform.position);
            clientManager.SendServerUDPMessage(3, status);
            SendPositionTimeout = Time.time + 0.06f;
        }
    }
}
