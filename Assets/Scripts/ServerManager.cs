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
    private PlayerFinder playerFinder;

    public Dictionary<string, IPEndPoint> Clients = new Dictionary<string, IPEndPoint>(); 

    void Awake() {
        // Desactiver mon objet si je ne suis pas le serveur
        if (!Globals.IsServer) {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        playerFinder = GameObject.FindFirstObjectByType<PlayerFinder>();
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
                        GameObject playerSpawned = playerSpawner.SpawnPlayer(addr, false);
                        playerFinder.RegisterPlayer(addr, playerSpawned);


                        foreach (KeyValuePair<string, IPEndPoint> client in Clients)
                        {
                            if (client.Key != addr) {
                                PayloadSpawnPlayer spawnExternal = new PayloadSpawnPlayer { id = client.Key };
                                byte[] bytesSpawnExternal = UDP.ObjectToByteArray(2, spawnExternal);
                                UDP.SendUDPBytes(bytesSpawnExternal, sender);
                            }
                        }
                        BroadcastUDPMessage(2, spawn, addr);
                        break;
                    case 3://players positions

                        PayloadPlayerStatus playerStatus = UDP.FromByteArray<PayloadPlayerStatus>(message);
                        GameObject playerToMove = playerFinder.FindPlayerByID(playerStatus.id);

                        playerToMove.transform.position = playerStatus.GetPosition();
                        playerToMove.transform.rotation = playerStatus.GetRotation();

                        Animator animator = playerToMove.GetComponent<Animator>();
                        if (animator != null)
                        {
                            Debug.Log("animator not null and isMoving: " + playerStatus.GetIsMoving());
                            animator.SetFloat("walk", playerStatus.GetIsMoving() ? 1f : 0f);
                            animator.SetBool("isBacking", false);
                        }
                        BroadcastUDPMessage(3, playerStatus, playerStatus.id);
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
            if (client.Key != clientId)
            {
                byte[] bytes = UDP.ObjectToByteArray(type, obj);
                UDP.SendUDPBytes(bytes, client.Value);
            }
        }
    }
}
