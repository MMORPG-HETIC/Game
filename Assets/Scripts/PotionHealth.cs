using UnityEngine;

public class PotionHealth : MonoBehaviour
{
    public int healAmount = 20; // Quantité de vie restaurée par la potion

    public delegate void PotionUsedHandler();
    public event PotionUsedHandler OnPotionUsed;

    void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'objet entrant est un joueur avec PlayerHealth
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Restaurer la vie du joueur
            playerHealth.Heal(healAmount);

            // Déclencher l'événement pour signaler que la potion a été utilisée
            OnPotionUsed?.Invoke();

            // Détruire la potion après utilisation
            Destroy(gameObject);

            Debug.Log("Potion utilisée, vie restaurée !");
        }
    }
}
