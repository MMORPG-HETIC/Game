using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [Header("Potion Configuration")]
    public GameObject potionPrefab; // Le préfabriqué de la potion à générer
    public int maxPotions = 10; // Nombre maximum de potions sur la carte
    public float spawnRadius = 5f; // Rayon de la zone de spawn
    public float spawnInterval = 10f; // Temps entre chaque tentative de spawn (en secondes)

    [Header("Spawn Area Configuration")]
    public Vector3 spawnCenter = Vector3.zero; // Centre de la zone de spawn
    public float spawnHeight = 0f; // Hauteur à laquelle les potions apparaissent

    private int currentPotionCount = 0; // Nombre de potions actuellement actives

    void Start()
    {
        // Vérifier que le préfabriqué est assigné
        if (potionPrefab == null)
        {
            Debug.LogError("PotionPrefab n'est pas assigné dans l'inspecteur !");
            return;
        }

        // Lancer le spawn automatique
        InvokeRepeating(nameof(SpawnPotion), 0f, spawnInterval);
    }

    void SpawnPotion()
    {
        // Vérifier si le nombre de potions est déjà au maximum
        if (currentPotionCount >= maxPotions) return;

        // Générer une position aléatoire dans le rayon défini
        Vector3 randomPosition = GetRandomSpawnPosition();

        // Instancier la potion
        GameObject spawnedPotion = Instantiate(potionPrefab, randomPosition, Quaternion.identity);

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

        Debug.Log($"Potion générée à {randomPosition}");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Générer une position aléatoire dans un cercle et l'appliquer autour du spawnCenter
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        return new Vector3(spawnCenter.x + randomCircle.x, spawnHeight, spawnCenter.z + randomCircle.y);
    }

    private void HandlePotionUsed()
    {
        // Réduire le compteur de potions actives
        currentPotionCount--;
    }
}
