using System.Net;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public UDPService UDP;
    public string ServerIP = "127.0.0.1";
    public int ServerPort = 25000;

    private float NextCoucouTimeout = -1;
    private float SendPositionTimeout = -1;
    private IPEndPoint ServerEndpoint;
    private string id;
    private GameObject Player;
    private GameObject Zombie;
    public PlayerSpawner playerSpawner;

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
                sender.Address.ToString() + ":" + sender.Port);

            switch (message[0]) {
                case 0:
                    NextCoucouTimeout = Time.time + 20;
                    PayloadCheck check = UDP.FromByteArray<PayloadCheck>(message);
                    id = check.id;
                    break;
                case 1:
                    PayloadPlayerStatus playerStatus= UDP.FromByteArray<PayloadPlayerStatus>(message);
                    //
                    break;
                case 2:
                    PayloadZombieStatus zombieStatus = UDP.FromByteArray<PayloadZombieStatus>(message);
                    Zombie.transform.position = zombieStatus.GetPosition();
                    break;
            }
        };


        playerSpawner.SpawnPlayer();
        Debug.Log("hello there");
        Player = GameObject.Find("Player 1");
        Zombie = GameObject.Find("Zombie_0");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > NextCoucouTimeout) {
            byte[] bytes = { 0 };
            UDP.SendUDPBytes(bytes, ServerEndpoint);
            NextCoucouTimeout = Time.time + 0.5f;
        }

        if (string.IsNullOrEmpty(id)) { return; }

        if (Time.time > SendPositionTimeout)
        {
            PayloadPlayerStatus status = new PayloadPlayerStatus { id = id };
            status.SetPosition(Player.transform.position);
            byte[] bytesArray = UDP.ObjectToByteArray(1, status);
            UDP.SendUDPBytes(bytesArray, ServerEndpoint);
            SendPositionTimeout = Time.time + 0.06f;
        }
    }
}
