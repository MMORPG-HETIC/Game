using System;
using System.Collections;
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
    private ZombieFinder zombieFinder;

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
        zombieFinder = GameObject.FindFirstObjectByType<ZombieFinder>();
        UDP.Listen(ListenPort);

        UDP.OnMessageReceived +=  
            (byte[] message, IPEndPoint sender) => {
                switch (message[0]) {
                    case 0://getCoucou
                        // Ajouter le client à mon dictionnaire
                        string addr = sender.Address.ToString() + ":" + sender.Port;
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

                        PayloadSpawnPlayer spawn = new PayloadSpawnPlayer { id = addr };
                        byte[] bytes = UDP.ObjectToByteArray(1, spawn);
                        UDP.SendUDPBytes(bytes, sender);

                        GameObject playerSpawned = playerSpawner.SpawnPlayer(addr, false);
                        GameObject zombieSpawned = zombieSpawner.SpawnZombie(addr);
                        playerFinder.RegisterPlayer(addr, playerSpawned);
                        zombieFinder.RegisterZombie(addr, zombieSpawned);


                        foreach (KeyValuePair<string, IPEndPoint> client in Clients)
                        {
                            if (client.Key != addr) {
                                PayloadSpawnPlayer spawnExternal = new PayloadSpawnPlayer { id = client.Key };
                                byte[] bytesSpawnExternal = UDP.ObjectToByteArray(2, spawnExternal);
                                UDP.SendUDPBytes(bytesSpawnExternal, sender);
                            }
                        }

                        foreach (var zombie in zombieFinder.GetAllZombies())
                        {
                            ZombieAttribute zombieAttribute = zombie.GetComponent<ZombieAttribute>();
                            PayloadZombieSpawn existingZombieSpawn = new PayloadZombieSpawn{id = zombieAttribute.ID};
                            byte[] bytesZombie = UDP.ObjectToByteArray(4, existingZombieSpawn);
                            UDP.SendUDPBytes(bytesZombie, sender);
                        }

                        BroadcastUDPMessage(2, spawn, addr);

                        PayloadZombieSpawn spawnZombie = new PayloadZombieSpawn { id = addr };
                        BroadcastUDPMessage(4, spawnZombie, addr);
                        break;
                    case 3://players positions

                        PayloadPlayerStatus playerStatus = UDP.FromByteArray<PayloadPlayerStatus>(message);
                        GameObject playerToMove = playerFinder.FindPlayerByID(playerStatus.id);

                        playerToMove.transform.position = playerStatus.GetPosition();
                        playerToMove.transform.rotation = playerStatus.GetRotation();

                        Animator animator = playerToMove.GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.SetFloat("walk", playerStatus.GetIsMoving() ? 1f : 0f);
                            animator.SetBool("isBacking", false);
                        }
                        BroadcastByte(message, playerStatus.id);
                        //BroadcastUDPMessage(3, playerStatus, playerStatus.id);
                        break;
                    //case 6:
                    //    string addrKiller = sender.Address.ToString() + ":" + sender.Port;
                    //    PayloadCheck zombieDead = UDP.FromByteArray<PayloadCheck>(message);
                    //    zombieFinder.RemoveZombie(zombieDead.id);
                    //    BroadcastUDPMessage(6, zombieDead, addrKiller);
                    //    GameObject resurrectZombie = zombieSpawner.SpawnZombie(zombieDead.id);
                    //    zombieFinder.RegisterZombie(zombieDead.id, resurrectZombie);

                    //    PayloadZombieSpawn spawnResurectZombie = new PayloadZombieSpawn { id = zombieDead.id };
                    //    BroadcastUDPMessage(4, spawnResurectZombie);
                    //    break;
                    case 6:
                        string addrKiller = sender.Address.ToString() + ":" + sender.Port;
                        PayloadCheck zombieDead = UDP.FromByteArray<PayloadCheck>(message);

                        // Lancer la coroutine pour gérer la suppression et le respawn
                        StartCoroutine(HandleZombieRespawn(addrKiller, zombieDead));
                        break;

                    case 9://playerFinder quit
                        PayloadCheck quit = UDP.FromByteArray<PayloadCheck>(message);
                        playerFinder.RemovePlayer(quit.id);
                        Clients.Remove(quit.id);
                        BroadcastUDPMessage(9, quit, quit.id);
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
        byte[] bytes = UDP.ObjectToByteArray(type, obj);
        foreach (KeyValuePair<string, IPEndPoint> client in Clients) {
            if (client.Key != clientId)
            {
                UDP.SendUDPBytes(bytes, client.Value);
            }
        }
    }

    public void BroadcastByte(byte[] message, string clientId = "")
    {
        foreach (KeyValuePair<string, IPEndPoint> client in Clients)
        {
            if (client.Key != clientId)
            {
                UDP.SendUDPBytes(message, client.Value);
            }
        }
    }

    private IEnumerator HandleZombieRespawn(string addrKiller, PayloadCheck zombieDead)
    {
        zombieFinder.RemoveZombie(zombieDead.id);
        BroadcastUDPMessage(6, zombieDead, addrKiller);

        yield return new WaitForSeconds(2);

        GameObject resurrectZombie = zombieSpawner.SpawnZombie(zombieDead.id);
        zombieFinder.RegisterZombie(zombieDead.id, resurrectZombie);

        PayloadZombieSpawn spawnResurrectZombie = new PayloadZombieSpawn { id = zombieDead.id };
        BroadcastUDPMessage(4, spawnResurrectZombie);
    }

}
