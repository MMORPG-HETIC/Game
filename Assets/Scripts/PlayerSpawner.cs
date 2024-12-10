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

    private GameObject gameOverCanvas; // Canvas du GameOver
    private GameObject controllablePlayer;
    private List<GameObject> aiPlayers = new List<GameObject>();

   void Start()
{
    // Vérifications initiales
    if (spawnCenter == null || playerPrefab == null || aiPrefab == null)
    {
        Debug.LogError("Certains objets ne sont pas assignés dans l'inspecteur !");
        return;
    }

    // Trouver le GameOver Canvas dans la hiérarchie du Canvas parent
    Transform canvasTransform = GameObject.Find("Canvas")?.transform;
    if (canvasTransform != null)
    {
        Transform gameOverTransform = canvasTransform.Find("gameOverCanvas");
        if (gameOverTransform != null)
        {
            gameOverCanvas = gameOverTransform.gameObject;
            gameOverCanvas.SetActive(false); // Désactiver au début
            Debug.Log("GameOverCanvas trouvé et désactivé.");
        }
        else
        {
            Debug.LogError("GameOverCanvas introuvable dans le Canvas !");
        }
    }
    else
    {
        Debug.LogError("Canvas introuvable dans la scène !");
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
            10,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Vector3 spawnPosition = spawnCenter.position + randomOffset;

        if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            spawnPosition.y = hit.point.y;
        }

        return spawnPosition;
    }

    void SetupControllablePlayer(GameObject player)
    {
        player.name = "ControllablePlayer";

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Trouver la HealthBar automatiquement
            HealthBar healthBar = Object.FindFirstObjectByType<HealthBar>();

            // Passer le GameOver Canvas et la HealthBar au joueur
            playerHealth.Initialize(true, healthBar, gameOverCanvas);
            Debug.Log("GameOverCanvas assigné au joueur principal.");
        }
        else
        {
            Debug.LogError("Le script PlayerHealth est introuvable sur le joueur.");
        }

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

        PlayerHealth playerHealth = aiPlayer.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = aiPlayer.AddComponent<PlayerHealth>();
        }

        playerHealth.maxHealth = Random.Range(50, 150);
        playerHealth.Initialize(false);

        AIPlayerMovement aiMovement = aiPlayer.GetComponent<AIPlayerMovement>();
        if (aiMovement == null)
        {
            aiPlayer.AddComponent<AIPlayerMovement>();
        }
    }
}
