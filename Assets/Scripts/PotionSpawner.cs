using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [Header("Potion Configuration")]
    public GameObject potionPrefab; // Le préfabriqué de la potion à générer
    public int maxPotions = 10; // Nombre maximum de potions sur la carte
    public float spawnInterval = 10f; // Temps entre chaque tentative de spawn (en secondes)

    [Header("Spawn Area Configuration")]
    public Transform spawnCenter; // Transform définissant le centre de la zone de spawn
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10); // Taille de la zone de spawn
    public float spawnHeight = 10f; // Hauteur initiale pour le raycast

    private int currentPotionCount = 0; // Nombre de potions actuellement actives

    void Start()
    {
        // Vérifier que le préfabriqué est assigné
        if (potionPrefab == null)
        {
            Debug.LogError("PotionPrefab n'est pas assigné dans l'inspecteur !");
            return;
        }

        if (spawnCenter == null)
        {
            Debug.LogError("Le spawnCenter n'est pas assigné dans l'inspecteur !");
            return;
        }

        // Lancer le spawn automatique
        InvokeRepeating(nameof(SpawnPotion), 0f, spawnInterval);
    }

    void SpawnPotion()
    {
        // Vérifier si le nombre de potions est déjà au maximum
        if (currentPotionCount >= maxPotions) return;

        // Générer une position dans la zone définie
        Vector3 spawnPosition = GetGroundedSpawnPosition();

        // Instancier la potion
        GameObject spawnedPotion = Instantiate(potionPrefab, spawnPosition, Quaternion.identity);

        // Vérifier si le préfabriqué contient bien le script PotionHealth
        PotionHealth potionHealth = spawnedPotion.GetComponent<PotionHealth>();
        if (potionHealth == null)
        {
            Debug.LogError($"Le préfabriqué {potionPrefab.name} n'a pas de composant PotionHealth !");
            Destroy(spawnedPotion);
            return;
        }

        // Ajouter un événement pour réduire le compteur quand la potion est utilisée
        potionHealth.OnPotionUsed += HandlePotionUsed;

        // Incrémenter le compteur de potions actives
        currentPotionCount++;

        Debug.Log($"Potion générée à {spawnPosition}");
    }

    private Vector3 GetGroundedSpawnPosition()
    {
        // Calculer une position aléatoire dans la zone définie
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnHeight,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Vector3 spawnPosition = spawnCenter.position + randomOffset;

        // Ajuster la hauteur pour positionner sur le sol (si applicable)
        if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            spawnPosition.y = hit.point.y;
        }

        return spawnPosition;
    }

    private void HandlePotionUsed()
    {
        // Réduire le compteur de potions actives
        currentPotionCount--;
    }
}