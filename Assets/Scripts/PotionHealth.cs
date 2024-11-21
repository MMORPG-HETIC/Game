using UnityEngine;

public class PotionHealth : MonoBehaviour
{
    public int healAmount = 20; // Quantité de vie restaurée par la potion

    void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'objet qui entre en collision est le joueur
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Restaurer la vie du joueur
            playerHealth.Heal(healAmount);

            // Détruire la potion après utilisation
            Destroy(gameObject);

            Debug.Log("Potion utilisée, vie restaurée !");
        }
    }
}
