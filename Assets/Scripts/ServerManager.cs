using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public UDPService UDP;
    public int ListenPort = 25000;
    private float SendZombiePositionTimeout = -1;
    private GameObject Zombie;
    public ZombieSpawner zombieSpawner;
    public PlayerSpawner playerSpawner;

    public Dictionary<string, IPEndPoint> Clients = new Dictionary<string, IPEndPoint>(); 

    void Awake() {
        // Desactiver mon objet si je ne suis pas le serveur
        if (!Globals.IsServer) {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        UDP.Listen(ListenPort);

        UDP.OnMessageReceived +=  
            (byte[] message, IPEndPoint sender) => {
                Debug.Log("[SERVER] Message received from " + 
                    sender.Address.ToString() + ":" + sender.Port 
                    + " =>" + message);
                
                switch (message[0]) {
                    case 0:
                        // Ajouter le client à mon dictionnaire
                        string addr = sender.Address.ToString() + ":" + sender.Port;
                        if (!Clients.ContainsKey(addr)) {
                            Clients.Add(addr, sender);
                        }
                        Debug.Log("There are " + Clients.Count + " clients present.");

                        PayloadCheck check = new PayloadCheck { id = addr };
                        byte[] bytes = UDP.ObjectToByteArray(0, check);
                        UDP.SendUDPBytes(bytes, sender);

                        zombieSpawner.SpawnZombie();
                        playerSpawner.SpawnPlayer();
                        break;
                    case 1:
                        PayloadPlayerStatus playerstatus = UDP.FromByteArray<PayloadPlayerStatus>(message);
                        BroadcastUDPMessage(1, playerstatus, playerstatus.id);
                        break;
                }
            };
        Zombie = GameObject.Find("Zombie_0");
    }

    void Update()
    {
        if (Time.time > SendZombiePositionTimeout)
        {
            PayloadZombieStatus zombieStatus = new PayloadZombieStatus { id = 0 };
            zombieStatus.SetPosition(Zombie.transform.position);
            BroadcastUDPMessage(2, zombieStatus);
            SendZombiePositionTimeout = Time.time + 0.06f;
        }
    }

    public void BroadcastUDPMessage<T>(byte type, T obj, string clientId = "") {
        foreach (KeyValuePair<string, IPEndPoint> client in Clients) {
            if (client.Key == clientId) { return; }
            byte[] bytes = UDP.ObjectToByteArray(type, obj);
            UDP.SendUDPBytes(bytes, client.Value);
        }
    }
}
