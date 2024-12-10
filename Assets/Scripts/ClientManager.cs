using System.Net;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public UDPService UDP;
    public string ServerIP = "192.168.43.241";
    public int ServerPort = 25000;

    private float NextCoucouTimeout = -1;
    private IPEndPoint ServerEndpoint;
    private string id;
    private GameObject Player;
    private GameObject Zombie;
    public PlayerSpawner playerSpawner;
    private PlayerFinder playerFinder;

    void Awake() {
        // Desactiver mon objet si je ne suis pas le client
        if (Globals.IsServer) {
            gameObject.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UDP.InitClient();

        ServerEndpoint = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
            
        UDP.OnMessageReceived += (byte[] message, IPEndPoint sender) => {
            Debug.Log("[CLIENT] Message received from " + 
                sender.Address.ToString() + ":" + sender.Port + "message " + message[0]);

            switch (message[0]) {
                case 0://checkCoucou
                    NextCoucouTimeout = Time.time + 20;
                    break;
                case 1: //spawnPlayer
                    PayloadSpawnPlayer spawn = UDP.FromByteArray<PayloadSpawnPlayer>(message);
                    id = spawn.id;
                    playerSpawner.SpawnPlayer(id, true);
                    //GameObject Player = playerFinder.FindPlayerByIP(id);
                    break;
                case 2://spawnPlayer external
                    PayloadSpawnPlayer ExternalPlayerToSpawn = UDP.FromByteArray<PayloadSpawnPlayer>(message);
                    Debug.Log("payloadSpawner" + ExternalPlayerToSpawn);
                    playerSpawner.SpawnPlayer(ExternalPlayerToSpawn.id, false);
                    break;
                case 3://playersPositions
                    PayloadPlayerStatus playerStatus = UDP.FromByteArray<PayloadPlayerStatus>(message);
                    GameObject ExternalPlayerToMove = playerFinder.FindPlayerByIP(playerStatus.id);
                    ExternalPlayerToMove.transform.position = playerStatus.GetPosition();
                    break;
                case 4://zombiePosition
                    PayloadZombieStatus zombieStatus = UDP.FromByteArray<PayloadZombieStatus>(message);
                    Zombie.transform.position = zombieStatus.GetPosition();
                    break;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > NextCoucouTimeout) {
            byte[] bytes = { 0 };
            UDP.SendUDPBytes(bytes, ServerEndpoint);
            NextCoucouTimeout = Time.time + 0.5f;
        }
    }

    public void SendServerUDPMessage<T>(byte type, T obj)
    {
        byte[] bytes = UDP.ObjectToByteArray(type, obj);
            UDP.SendUDPBytes(bytes, ServerEndpoint);
    }
}
