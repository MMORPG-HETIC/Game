using System.Net;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public UDPService UDP;
    //public string ServerIP = Globals.IPServer;
    private int ServerPort = 25000;

    private float NextCoucouTimeout = -1;
    private IPEndPoint ServerEndpoint;
    private string id;
    private GameObject Player;
    private GameObject Zombie;
    public PlayerSpawner playerSpawner;
    public ZombieSpawner zombieSpawner;
    private PlayerFinder playerFinder;
    private ZombieFinder zombieFinder;

    void Awake() {
        // Desactiver mon objet si je ne suis pas le client
        if (Globals.IsServer) {
            gameObject.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerFinder = GameObject.FindFirstObjectByType<PlayerFinder>();
        zombieFinder = GameObject.FindFirstObjectByType<ZombieFinder>();
        UDP.InitClient();

        ServerEndpoint = new IPEndPoint(IPAddress.Parse(Globals.IPServer), ServerPort);
            
        UDP.OnMessageReceived += (byte[] message, IPEndPoint sender) => {
            switch (message[0]) {
                case 0://checkCoucou
                    NextCoucouTimeout = Time.time + 20;
                    break;
                case 1: //spawnPlayer
                    PayloadSpawnPlayer spawn = UDP.FromByteArray<PayloadSpawnPlayer>(message);
                    id = spawn.id;
                    GameObject player = playerSpawner.SpawnPlayer(id, true);
                    break;
                case 2://spawnPlayer external
                    PayloadSpawnPlayer ExternalPlayerToSpawn = UDP.FromByteArray<PayloadSpawnPlayer>(message);
                    GameObject externalPlayer = playerSpawner.SpawnPlayer(ExternalPlayerToSpawn.id, false);
                    playerFinder.RegisterPlayer(ExternalPlayerToSpawn.id, externalPlayer);
                    break;
                case 3://playersPositions
                    PayloadPlayerStatus playerStatus = UDP.FromByteArray<PayloadPlayerStatus>(message);
                    GameObject ExternalPlayerToMove = playerFinder.FindPlayerByID(playerStatus.id);
                    ExternalPlayerToMove.transform.position = playerStatus.GetPosition();
                    ExternalPlayerToMove.transform.rotation = playerStatus.GetRotation();
                    Animator animator = ExternalPlayerToMove.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetFloat("walk", playerStatus.GetIsMoving() ? 1f : 0f);
                        animator.SetBool("isBacking", false);
                    }
                    //ExternalPlayerToMove.transform.position = playerStatus.GetPosition();
                    break;
                case 4: // Zombie spawn
                    PayloadZombieSpawn zombieSpawn = UDP.FromByteArray<PayloadZombieSpawn>(message);

                    GameObject existingZombie = zombieFinder.FindZombieByID(zombieSpawn.id);
                    if (existingZombie != null)
                    {
                        zombieFinder.RemoveZombie(zombieSpawn.id);
                    }

                    GameObject zombieSpawned = zombieSpawner.SpawnZombie(zombieSpawn.id);
                    if (zombieSpawned != null)
                    {
                        zombieFinder.RegisterZombie(zombieSpawn.id, zombieSpawned);
                    }
                    else
                    {
                        Debug.LogError("Echec du spawn pour le zombie avec ID : " + zombieSpawn.id);
                    }
                    break;
                case 5://zombiePosition
                    PayloadZombieStatus zombieStatus = UDP.FromByteArray<PayloadZombieStatus>(message);
                    GameObject zombieToMove = zombieFinder.FindZombieByID(zombieStatus.id);
                    zombieToMove.transform.position = zombieStatus.GetPosition();
                    zombieToMove.transform.rotation = zombieStatus.GetRotation();
                    break;
                case 6: //Zombie dead
                    PayloadCheck zombieDead = UDP.FromByteArray<PayloadCheck>(message);
                    zombieFinder.RemoveZombie(zombieDead.id);
                    break;
                case 9://playerFinder quit
                    PayloadCheck quit = UDP.FromByteArray<PayloadCheck>(message);
                    playerFinder.RemovePlayer(quit.id);
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
