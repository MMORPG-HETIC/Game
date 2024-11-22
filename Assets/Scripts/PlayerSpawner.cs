using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Spawner Configuration")]
    public GameObject playerPrefab;
    public GameObject aiPrefab;
    public Transform spawnCenter;
    public Vector3 spawnAreaSize = new Vector3(20, 0, 20);
    public int numberOfAIPlayers = 3;

    private GameObject controllablePlayer;
    private List<GameObject> aiPlayers = new List<GameObject>();

    void Start()
    {
        if (spawnCenter == null)
        {
            Debug.LogError("Le spawnCenter n'est pas assigné dans l'inspecteur. Assurez-vous de le configurer.");
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("Le playerPrefab n'est pas assigné dans l'inspecteur. Assurez-vous de le configurer.");
            return;
        }

        if (aiPrefab == null)
        {
            Debug.LogError("Le aiPrefab n'est pas assigné dans l'inspecteur. Assurez-vous de le configurer.");
            return;
        }

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        // Spawn du joueur contrôlé
        Vector3 playerSpawnPosition = GetGroundedSpawnPosition();
        controllablePlayer = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        SetupControllablePlayer(controllablePlayer);

        // Spawn des avatars IA
        for (int i = 0; i < numberOfAIPlayers; i++)
        {
            Vector3 aiSpawnPosition = GetGroundedSpawnPosition();
            GameObject aiPlayer = Instantiate(aiPrefab, aiSpawnPosition, Quaternion.identity);
            SetupAIPlayer(aiPlayer, i);
        }
    }

    Vector3 GetGroundedSpawnPosition()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            10, // Hauteur initiale pour le raycast
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Vector3 spawnPosition = spawnCenter.position + randomOffset;

        if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            spawnPosition.y = hit.point.y; // Positionner sur le sol
        }

        return spawnPosition;
    }

    void SetupControllablePlayer(GameObject player)
    {
        player.name = "ControllablePlayer";

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Initialize(true, Object.FindFirstObjectByType<HealthBar>()); // Joueur principal
        }

        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = true; // Activer le contrôle utilisateur
        }
    }

    void SetupAIPlayer(GameObject aiPlayer, int index)
    {
        aiPlayer.name = $"AIPlayer_{index}";
        aiPlayers.Add(aiPlayer);

        PlayerHealth playerHealth = aiPlayer.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = aiPlayer.AddComponent<PlayerHealth>();
        }

        playerHealth.maxHealth = Random.Range(50, 150);
        playerHealth.Initialize(false); // IA

        AIPlayerMovement aiMovement = aiPlayer.GetComponent<AIPlayerMovement>();
        if (aiMovement == null)
        {
            aiPlayer.AddComponent<AIPlayerMovement>();
        }

        PlayerController controller = aiPlayer.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false; // Désactiver le contrôle utilisateur
        }
    }
}
