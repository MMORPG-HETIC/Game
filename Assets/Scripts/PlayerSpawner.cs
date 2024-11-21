using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Spawner Configuration")]
    public GameObject playerPrefab; // Préfabriqué du joueur contrôlé
    public GameObject aiPrefab; // Préfabriqué des avatars IA
    public Transform spawnCenter; // Centre de la zone de spawn
    public Vector3 spawnAreaSize = new Vector3(20, 0, 20); // Taille de la zone de spawn
    public int numberOfAIPlayers = 3; // Nombre d'avatars IA à générer

    private GameObject controllablePlayer; // Référence au joueur contrôlable
    private List<GameObject> aiPlayers = new List<GameObject>(); // Liste des joueurs IA

    void Start()
    {
        // Vérification des configurations nécessaires
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
        // Générer une position aléatoire dans la zone de spawn
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            10, // Initialisation à une certaine hauteur pour le raycast
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Vector3 spawnPosition = spawnCenter.position + randomOffset;

        // Ajuster la position pour être au niveau du sol
        if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            spawnPosition.y = hit.point.y; // Positionner sur le sol
        }

        return spawnPosition;
    }

    void SetupControllablePlayer(GameObject player)
    {
        player.name = "ControllablePlayer";

        // Configurer la barre de santé uniquement pour le joueur contrôlé
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.healthBar = FindObjectOfType<HealthBar>();
        }

        // Activer le contrôle utilisateur
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = true;
        }
    }

    void SetupAIPlayer(GameObject aiPlayer, int index)
    {
        aiPlayer.name = $"AIPlayer_{index}";
        aiPlayers.Add(aiPlayer);

        // Ajouter un script IA pour gérer le comportement
        AIPlayerMovement aiMovement = aiPlayer.GetComponent<AIPlayerMovement>();
        if (aiMovement == null)
        {
            aiPlayer.AddComponent<AIPlayerMovement>();
        }

        // Désactiver les scripts inutiles pour les IA
        PlayerController controller = aiPlayer.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Les IA n'ont pas besoin de barre de santé
        PlayerHealth playerHealth = aiPlayer.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.healthBar = null;
        }
    }
}
