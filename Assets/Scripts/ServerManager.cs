using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public UDPService UDP;
    public int ListenPort = 25000;
    //private float SendZombiePositionTimeout = -1;
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
                    + " =>" + message[0]);
                
                switch (message[0]) {
                    case 0://getCoucou
                        // Ajouter le client à mon dictionnaire
                        string addr = sender.Address.ToString() + ":" + sender.Port;
                        Debug.Log(addr);
                        PayloadCheck check = new PayloadCheck { id = addr };
                        byte[] bytesCheck = UDP.ObjectToByteArray(0, check);
                        UDP.SendUDPBytes(bytesCheck, sender);
                        if (Clients.ContainsKey(addr))
                        {
                            break;
                        }
                        if (!Clients.ContainsKey(addr)) {
                            Clients.Add(addr, sender);
                        }
                        Debug.Log("There are " + Clients.Count + " clients present. : " + addr);

                        PayloadSpawnPlayer spawn = new PayloadSpawnPlayer { id = addr };
                        byte[] bytes = UDP.ObjectToByteArray(1, spawn);
                        UDP.SendUDPBytes(bytes, sender);

                        //zombieSpawner.SpawnZombie();
                        playerSpawner.SpawnPlayer(addr, false);
                        foreach (KeyValuePair<string, IPEndPoint> client in Clients)
                        {
                            if (client.Value == sender) { return; }
                            byte[] bytesSpawnExternal = UDP.ObjectToByteArray(2, spawn);
                            UDP.SendUDPBytes(bytes, sender);
                        }
                        BroadcastUDPMessage(2, addr);
                        break;
                    case 3://players positions
                        PayloadPlayerStatus playerstatus = UDP.FromByteArray<PayloadPlayerStatus>(message);
                        BroadcastUDPMessage(3, playerstatus, playerstatus.id);
                        break;
                }
            };
        //Zombie = GameObject.Find("Zombie_0");
    }

    void Update()
    {
        //if (Time.time > SendZombiePositionTimeout)
        //{
        //    PayloadZombieStatus zombieStatus = new PayloadZombieStatus { id = 0 };
        //    zombieStatus.SetPosition(Zombie.transform.position);
        //    BroadcastUDPMessage(2, zombieStatus);
        //    SendZombiePositionTimeout = Time.time + 0.06f;
        //}
    }

    public void BroadcastUDPMessage<T>(byte type, T obj, string clientId = "") {
        foreach (KeyValuePair<string, IPEndPoint> client in Clients) {
            if (client.Key == clientId) { return; }
            byte[] bytes = UDP.ObjectToByteArray(type, obj);
            UDP.SendUDPBytes(bytes, client.Value);
        }
    }
}
